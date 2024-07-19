using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using DataAccessLayer;
using Service.Interfaces;
using Service;

namespace M_BMilkStoreClient.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IProductLineService _productLineService;

        public IndexModel(IProductService productService, IProductLineService productLineService)
        {
            _productService = productService;
            _productLineService = productLineService;
        }

        public IList<Product> Product { get; set; } = default!;
        public int PageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Staff")
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            PageIndex = pageIndex ?? 1;

            var pagedResult = await _productService.GetProductsPagedAsync(PageIndex, PageSize, SearchString);

            Product = pagedResult.Items;
            foreach (var product in Product)
            {
                var productLines = await _productLineService.GetProductLinesByProductId(product.ProductId);
                if (productLines != null && productLines.Any())
                {
                    product.TotalQuantity = productLines
                        .Where(pl => pl.Status == true && pl.IsDeleted == false)
                        .Sum(pl => pl.QuantityOut);
                }
            }
            TotalItems = pagedResult.TotalItems;
            TotalPages = pagedResult.TotalPages;

            return Page();
        }

    }
}