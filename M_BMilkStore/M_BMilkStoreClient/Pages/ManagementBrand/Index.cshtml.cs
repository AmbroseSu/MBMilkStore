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
    public class IndexModel : PageModel
    {
        private readonly IProductBrandService iProductBrandService;

        public IndexModel()
        {
            iProductBrandService = new ProductBrandService();
        }

        public IList<ProductBrand> ProductBrand { get;set; } = default!;
        public string MessageError { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ProductBrand = await iProductBrandService.GetProductBrands();
                return Page();
            }
            catch (Exception ex)
            {
                MessageError = $"Error: {ex.Message}";
                return Page();
            }
        }
    }
}