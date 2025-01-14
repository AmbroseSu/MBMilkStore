﻿using System;
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
    public class DeleteModel : PageModel
    {
        private readonly IProductBrandService iProductBrandService;
        private readonly IProductService iProductService;

        public DeleteModel()
        {
            iProductBrandService = new ProductBrandService();
            iProductService = new ProductService();
        }

        [BindProperty]
        public ProductBrand ProductBrand { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var product = await iProductService.GetProductByBrandId(id);
                var productbrand = await iProductBrandService.GetProductBrandById(id);
                if(product.Count != 0)
                {
                    ProductBrand = productbrand;
                    MessageError = "There are products that are using brands. Can not delete.";
                    return Page();
                }
                if (productbrand != null)
                {
                    ProductBrand = productbrand;
                    await iProductBrandService.DeleteProductBrand(ProductBrand);
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                MessageError = ex.Message;
                return Page();
            }
            
        }
    }
}
