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

namespace M_BMilkStoreClient.Pages.ManagementVoucher
{
    public class CreateModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        public bool IsAdmin => HttpContext.Session.GetString("UserRole") == "Admin";
        public CreateModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public IActionResult OnGet()
        {
            if (!IsAdmin)
            {
                return RedirectToPage("/Authenticate");
            }
            return Page();
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = default!;
        public string MessageError { get; set; } = string.Empty;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsAdmin)
            {
                return RedirectToPage("/Authenticate");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _voucherService.CreateVoucherAsync(Voucher);
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                MessageError = $"Error: {ex.Message}";
                return Page();
            }
        }
    }
}
