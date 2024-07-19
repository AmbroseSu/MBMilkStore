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

namespace M_BMilkStoreClient.Pages.ManagementProductLine
{
    public class DeleteModel : PageModel
    {
        private readonly IProductLineService iProductLineService;

        public DeleteModel()
        {
            iProductLineService = new ProductLineService();
        }

        [BindProperty]
        public ProductLine ProductLine { get; set; } = default!;

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

            var productline = await iProductLineService.GetProductLineById(id);

            if (productline == null)
            {
                return NotFound();
            }
            else
            {
                ProductLine = productline;
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

                var productline = await iProductLineService.GetProductLineById(id);
                if (productline != null)
                {
                    ProductLine = productline;
                    await iProductLineService.DeleteProductLine(productline);
                }

                return RedirectToPage("/Products/Index");
            }
            catch (Exception ex)
            {
                MessageError = ex.Message;
                return Page();
            }
            
        }
    }
}
