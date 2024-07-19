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

namespace M_BMilkStoreClient.Pages.ManagementBrand
{
    public class EditModel : PageModel
    {
        private readonly IProductBrandService iProductBrandService;

        public EditModel()
        {
            iProductBrandService = new ProductBrandService();
        }

        [BindProperty]
        public ProductBrand ProductBrand { get; set; } = default!;

        public string MessageError { get; set; } = string.Empty;
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int id)
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
            if (id == null)
            {
                return NotFound();
            }

            var productbrand = await iProductBrandService.GetProductBrandById(id);
            if (productbrand == null)
            {
                return NotFound();
            }
            ProductBrand = productbrand;
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
                    await iProductBrandService.UpdateProductBrand(ProductBrand);
                }
                catch (DbUpdateConcurrencyException db)
                {
                    if (!(await ProductBrandExists(ProductBrand.ProductBrandId)))
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

        private async Task<bool> ProductBrandExists(int id)
        {
            ProductBrand = await iProductBrandService.GetProductBrandById(id);
            if (ProductBrand == null)
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
