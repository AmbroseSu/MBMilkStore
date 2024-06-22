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
using Microsoft.EntityFrameworkCore;

namespace M_BMilkStoreClient.Pages.ManagementBrand
{
    public class CreateModel : PageModel
    {

        private readonly IProductBrandService iProductBrandService;

        public CreateModel()
        {
            iProductBrandService = new ProductBrandService();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductBrand ProductBrand { get; set; } = default!;
        public string MessageError { get; set; } = string.Empty;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                await iProductBrandService.SaveProductBrand(ProductBrand);

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
