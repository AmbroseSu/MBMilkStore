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
    public class IndexModel : PageModel
    {
        private readonly IProductLineService iProductLineService;

        public IndexModel()
        {
            iProductLineService = new ProductLineService();
        }

        public IList<ProductLine> ProductLine { get;set; } = default!;

        public string MessageError { get; set; } = string.Empty;
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int productId)
        {
            try
            {
                UserRole = HttpContext.Session.GetString("UserRole");
                if (UserRole != "Staff"&&UserRole!=null)
                {
                    return RedirectToPage("/Error");
                }
                if (UserRole == null)
                {
                    return RedirectToPage("/Authenticate");
                }
                ProductLine = await iProductLineService.GetProductLinesByProductId(productId);
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
