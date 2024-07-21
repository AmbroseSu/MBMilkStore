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
using Service;

namespace M_BMilkStoreClient.Pages.ManagementCategory
{
    public class CreateModel : PageModel
    {

        private readonly IProductCategoryService iProductCategoryService;

        public CreateModel()
        {
            iProductCategoryService = new ProductCategoryService();
        }


        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        public string MessageError { get; set; } = string.Empty;
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
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
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                ProductCategory.ListProduct = null;
                var productCategory = iProductCategoryService.SaveProductCategory(ProductCategory);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                MessageError = $"Error: {ex.Message}";
                return Page();
            }


            
        }
    }
}
