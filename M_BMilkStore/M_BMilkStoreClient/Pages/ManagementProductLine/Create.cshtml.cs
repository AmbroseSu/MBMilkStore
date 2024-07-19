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
using Microsoft.CodeAnalysis;

namespace M_BMilkStoreClient.Pages.ManagementProductLine
{
    public class CreateModel : PageModel
    {
        private readonly IProductLineService iProductLineService;
        private readonly IProductService iProductService;

        public CreateModel()
        {
            iProductLineService = new ProductLineService();
            iProductService = new ProductService();
        }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int productId)
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
            ProductLine = new ProductLine { ProductId = productId };
            
            ViewData["ProductId"] = new SelectList(new List<Product> { await iProductService.GetProductById(productId) }, "ProductId", "Name");
            return Page();
        }

        [BindProperty]
        public ProductLine ProductLine { get; set; } = default!;

        public string MessageError { get; set; } = string.Empty;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var productLine = iProductLineService.SaveProductLine(ProductLine);
                return RedirectToPage("/Products/Index");

            }
            catch (Exception ex)
            {
                MessageError = $"Error: {ex.Message}";
                return Page();
            }

            
        }
    }
}
