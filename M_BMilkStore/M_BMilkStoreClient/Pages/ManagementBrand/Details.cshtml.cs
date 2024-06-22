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

namespace M_BMilkStoreClient.Pages.ManagementBrand
{
    public class DetailsModel : PageModel
    {
        private readonly IProductBrandService iProductBrandService;

        public DetailsModel()
        {
            iProductBrandService = new ProductBrandService();
        }

        public ProductBrand ProductBrand { get; set; } = default!;
        public string MessageError { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var productbrand = await iProductBrandService.GetProductBrandById(id);
                if (productbrand == null)
                {
                    return NotFound();
                }
                else
                {
                    ProductBrand = productbrand;
                }
                return Page();
            }
            catch (Exception ex)
            {
                MessageError = ex.Message;
                return Page();
            }
            
        }
    }
}
