using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using DataAccessLayer;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Staff" && UserRole != null)
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["ProductBrandId"] = new SelectList(await _productService.GetAllProductBrand(), "ProductBrandId", "Name");
           ViewData["ProductCategoryId"] = new SelectList(await _productService.GetAllProductCategory(), "ProductCategoryId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _productService.UpdateProduct(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _productService.GetProductById(id) != null;
        }
    }
}
