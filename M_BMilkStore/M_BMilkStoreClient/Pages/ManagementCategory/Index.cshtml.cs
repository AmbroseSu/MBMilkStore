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

namespace M_BMilkStoreClient.Pages.ManagementCategory
{
    public class IndexModel : PageModel
    {
        private readonly IProductCategoryService iProductCategoryService;

        public IndexModel()
        {
            iProductCategoryService = new ProductCategoryService();
        }

        public IList<ProductCategory> ProductCategory { get;set; } = default!;

        public string MessageError { get; set; } = string.Empty;


        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ProductCategory = await iProductCategoryService.GetProductCategories();
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