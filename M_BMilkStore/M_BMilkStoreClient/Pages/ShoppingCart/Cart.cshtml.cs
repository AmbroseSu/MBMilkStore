using BussinessObject;
using M_BMilkStoreClient.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace M_BMilkStoreClient.Pages
{
    public class CartModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IProductLineService _lineService;

        public CartModel(IProductService productService, IProductLineService lineService)
        {
            _productService = productService;
            _lineService = lineService;
        }

        public IList<CartItem> Carts { get; set; } = new List<CartItem>();
        public Product Product { get; set; } = default!;

        [BindProperty]
        public double TotalPrice { get; set; }

        [BindProperty]
        public string DeliveryOptionName { get; set; }

        [BindProperty]
        public float DeliveryOptionValue { get; set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Customer" && UserRole != null)
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            LoadCartFromSession();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            LoadCartFromSession();

            int index = FindCartItemIndex(productId);

            if (index == -1)
            {
                Product = await _productService.GetProductCartById(productId);
                if (Product != null)
                {
                    Carts.Add(new CartItem { Product = Product, Quantity = 1 });
                }
            }
            else
            {
                int remainingQuantity = await _lineService.GetRemainingQuantityByProductId(productId);
                if (Carts[index].Quantity + 1 <= remainingQuantity)
                {
                    Carts[index].Quantity++;
                }
            }

            SaveCartToSession();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetIncreaseAsync(int id)
        {
            LoadCartFromSession();

            int index = FindCartItemIndex(id);
            if (index != -1)
            {
                int remainingQuantity = await _lineService.GetRemainingQuantityByProductId(id);

                if (Carts[index].Quantity + 1 <= remainingQuantity)
                {
                    Carts[index].Quantity++;
                    SaveCartToSession();
                    return new JsonResult(new { success = true });
                }
                else
                {
                    return new JsonResult(new { success = false, message = $"Only {remainingQuantity} items are available for this product." });
                }
            }

            return new JsonResult(new { success = false, message = "Product not found in the cart." });
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
            if (index != -1 && quantity >= 0)
            {
                int remainingQuantity = await _lineService.GetRemainingQuantityByProductId(id);

                if (quantity <= remainingQuantity)
                {
                    Carts[index].Quantity = quantity;
                    SaveCartToSession();
                    return new JsonResult(new { success = true });
                }
                else
                {
                    return new JsonResult(new { success = false, message = $"Only {remainingQuantity} items are available for this product." });
                }
            }

            return new JsonResult(new { success = false, message = "Product not found." });
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
            HttpContext.Session.SetString("TotalPrice", TotalPrice.ToString("F2"));
            HttpContext.Session.SetString("DeliveryOptionName", DeliveryOptionName);
            HttpContext.Session.SetString("DeliveryOptionValue", DeliveryOptionValue.ToString("F2"));

            return RedirectToPage("./Checkout");
        }
    }
}
