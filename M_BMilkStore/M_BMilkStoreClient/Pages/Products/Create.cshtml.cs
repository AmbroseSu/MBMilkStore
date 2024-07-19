using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject;
using DataAccessLayer;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;

        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Staff"&&UserRole!=null)
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            ViewData["ProductBrandId"] = new SelectList(await _productService.GetAllProductBrand(), "ProductBrandId", "Name");
        ViewData["ProductCategoryId"] = new SelectList(await _productService.GetAllProductCategory(), "ProductCategoryId", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ProductBrandId"] = new SelectList(await _productService.GetAllProductBrand(), "ProductBrandId", "Name");
                ViewData["ProductCategoryId"] = new SelectList(await _productService.GetAllProductCategory(), "ProductCategoryId", "Name");
                return Page();
            }

            try
            {
                await _productService.AddProduct(Product);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và hiển thị thông báo
                ModelState.AddModelError("", $"Error adding product: {ex.Message}");
                ViewData["ProductBrandId"] = new SelectList(await _productService.GetAllProductBrand(), "ProductBrandId", "Name");
                ViewData["ProductCategoryId"] = new SelectList(await _productService.GetAllProductCategory(), "ProductCategoryId", "Name");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
