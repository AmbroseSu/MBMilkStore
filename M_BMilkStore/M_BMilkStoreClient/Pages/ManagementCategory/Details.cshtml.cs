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

namespace M_BMilkStoreClient.Pages.ManagementCategory
{
    public class DetailsModel : PageModel
    {

        private readonly IProductCategoryService iProductCategoryService;

        public DetailsModel()
        {
            iProductCategoryService = new ProductCategoryService();
        }

        public ProductCategory ProductCategory { get; set; } = default!;

        public string MessageError { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var productcategory = await iProductCategoryService.GetProductCategoryById(id);
                if (productcategory == null)
                {
                    return NotFound();
                }
                else
                {
                    ProductCategory = productcategory;
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
