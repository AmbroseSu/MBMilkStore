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

namespace M_BMilkStoreClient.Pages.ManagementProductLine
{
    public class EditModel : PageModel
    {
        private readonly IProductLineService iProductLineService;
        private readonly IProductService iProductService;

        public EditModel()
        {
            iProductLineService = new ProductLineService();
            iProductService = new ProductService();
        }

        [BindProperty]
        public ProductLine ProductLine { get; set; } = default!;

        public string MessageError { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productline = await iProductLineService.GetProductLineById(id);
            if (productline == null)
            {
                return NotFound();
            }
            ProductLine = productline;
           ViewData["ProductId"] = new SelectList(await iProductService.GetAllProduct(), "ProductId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            try
            {
                await iProductLineService.UpdateProductLine(ProductLine);
            }
            catch (DbUpdateConcurrencyException db)
            {
                if (!(await ProductLineExists(ProductLine.ProductLineId)))
                {
                    return NotFound();
                }
                else
                {
                    MessageError = db.Message;
                    return Page();
                }
            }

            return RedirectToPage("/Products/Index");
        }

        private async Task<bool> ProductLineExists(int id)
        {
            ProductLine = await iProductLineService.GetProductLineById(id);
            if (ProductLine == null)
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
