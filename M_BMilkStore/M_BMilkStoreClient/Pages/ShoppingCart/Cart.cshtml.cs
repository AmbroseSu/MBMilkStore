using BussinessObject;
using M_BMilkStoreClient.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace M_BMilkStoreClient.Pages
{
    public class CartModel : PageModel
    {
        private readonly IProductService _productService;

        public CartModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<CartItem> Carts { get; set; } = new List<CartItem>();
        public Product Product { get; set; } = default!;

        [BindProperty]
        public double TotalPrice { get; set; }

        [BindProperty]
        public string DeliveryOptionName { get; set; }


        [BindProperty]
        public float DeliveryOptionValue { get; set; }
        public async Task OnGetAsync(int? id)
        {
            LoadCartFromSession();

            if (id.HasValue)
            {
                int index = FindCartItemIndex(id.Value);
                if (index == -1)
                {
                    Product = await _productService.GetProductCartById(id.Value);
                    if (Product != null)
                    {
                        Carts.Add(new CartItem { Product = Product, Quantity = 1 });
                    }
                }
                else
                {
                    Carts[index].Quantity++;
                }

                SaveCartToSession();
            }
        }

        public void OnGetIncrease(int id)
        {
            LoadCartFromSession();

            int index = FindCartItemIndex(id);
            if (index != -1)
            {
                Carts[index].Quantity++;
                SaveCartToSession();
            }
        }

        public void OnGetDelete(int id)
        {
            LoadCartFromSession();

            int index = FindCartItemIndex(id);
            if (index != -1)
            {
                Carts.RemoveAt(index);
                SaveCartToSession();
            }
        }

        public async Task<IActionResult> OnGetUpdateQuantityAsync(int id, int quantity)
        {
            LoadCartFromSession();

            int index = FindCartItemIndex(id);
            if (index != -1 && quantity > 0)
            {
                Carts[index].Quantity = quantity;
                SaveCartToSession();
            }

            return RedirectToPage();
        }

        private int FindCartItemIndex(int id)
        {
            for (int i = 0; i < Carts.Count; i++)
            {
                if (Carts[i]?.Product?.ProductId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private void LoadCartFromSession()
        {
            Carts = SessionService.GetSessionObjectAsJson<List<CartItem>>(HttpContext.Session, "cart") ?? new List<CartItem>();
        }

        private void SaveCartToSession()
        {
            SessionService.SetSessionObjectAsJson(HttpContext.Session, "cart", Carts);
        }

         public IActionResult OnPostProceedToCheckout()
        {
            // Store the total price and delivery option in the session
            HttpContext.Session.SetString("TotalPrice", TotalPrice.ToString("F2"));
            HttpContext.Session.SetString("DeliveryOptionName", DeliveryOptionName);
            HttpContext.Session.SetString("DeliveryOptionValue", DeliveryOptionValue.ToString("F2"));

            // Redirect to the checkout page
            return RedirectToPage("./Checkout");
        }
    }
}
