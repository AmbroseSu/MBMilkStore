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
using Service;

namespace M_BMilkStoreClient.Pages.ManagementCategory
{
    public class EditModel : PageModel
    {
        private readonly IProductCategoryService iProductCategoryService;

        public EditModel()
        {
            iProductCategoryService = new ProductCategoryService();
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        public string MessageError { get; set; } = string.Empty;
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int id)
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

            var productcategory = await iProductCategoryService.GetProductCategoryById(id);
            if (productcategory == null)
            {
                return NotFound();
            }
            ProductCategory = productcategory;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                

                try
                {
                    await iProductCategoryService.UpdateProductCategory(ProductCategory);
                }
                catch (DbUpdateConcurrencyException db)
                {
                    if (!(await ProductCategoryExists(ProductCategory.ProductCategoryId)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        MessageError = db.Message;
                        return Page();
                    }
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                MessageError = ex.Message;
                return Page();
            }
            
        }

        private async Task<bool> ProductCategoryExists(int id)
        {
            ProductCategory = await iProductCategoryService.GetProductCategoryById(id);
            if(ProductCategory == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
