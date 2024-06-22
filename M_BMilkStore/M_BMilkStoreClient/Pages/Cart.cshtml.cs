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

        public async Task OnGetAsync(int? id)
        {
            Carts = SessionService.GetSessionObjectAsJson<List<CartItem>>(HttpContext.Session, "cart") ?? new List<CartItem>();
            if (id.HasValue)
            {
                int index = Exists(Carts, id.Value);
                if (index == -1)
                {
                    Product = await _productService.GetProductById(id.Value);
                    Carts.Add(new CartItem { Product = Product, Quantity = 1 });
                }
                else
                {
                    Carts[index].Quantity++;
                }
                SessionService.SetSessionObjectAsJson(HttpContext.Session, "cart", Carts);
            }
        }

        private int Exists(IList<CartItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i]?.Product?.ProductId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public void OnGetIncrease(int id)
        {
            Carts = SessionService.GetSessionObjectAsJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = Exists(Carts, id);
            if (index != -1)
            {
                Carts[index].Quantity++;
                SessionService.SetSessionObjectAsJson(HttpContext.Session, "cart", Carts);
            }
        }

        public void OnGetDelete(int id)
        {
            Carts = SessionService.GetSessionObjectAsJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = Exists(Carts, id);
            if (index != -1)
            {
                Carts.RemoveAt(index);
                SessionService.SetSessionObjectAsJson(HttpContext.Session, "cart", Carts);
            }
        }
    }
}