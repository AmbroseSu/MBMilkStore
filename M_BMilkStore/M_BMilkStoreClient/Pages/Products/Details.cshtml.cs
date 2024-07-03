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
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace M_BMilkStoreClient.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IProductLineService iProductLineService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
            iProductLineService = new ProductLineService();
        }

        public Product Product { get; set; } = default!;
        public IList<ProductLine> ProductLines { get; set; } = default!;
        [BindProperty]
        public ProductLine ProductLine { get; set; }
        public string MessageError { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _productService.GetProductById(id.Value);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    Product = product;
                }
                ProductLines = await iProductLineService.GetProductLinesByProductId(id.Value);
                foreach (var productLine in ProductLines)
                {
                    if (productLine.ExpiredDate < DateTime.Now || productLine.ExpiredDate < DateTime.Now.AddDays(20))
                    {
                        productLine.Status = false;
                        await iProductLineService.UpdateProductLine(productLine); // Save the updated status
                    }
                }
                ViewData["ProductId"] = new SelectList(new List<Product> { Product }, "ProductId", "Name");
                ProductLine = new ProductLine { ProductId = id.Value };
                return Page();
            }
            catch (Exception ex)
            {
                MessageError = ex.Message;
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                ProductLine.ProductId = id;
                var productLine = iProductLineService.SaveProductLine(ProductLine);
                return RedirectToPage(new {id});

            }
            catch (Exception ex)
            {
                MessageError = $"Error: {ex.Message}";
                return Page();
            }


        }
    }
}
