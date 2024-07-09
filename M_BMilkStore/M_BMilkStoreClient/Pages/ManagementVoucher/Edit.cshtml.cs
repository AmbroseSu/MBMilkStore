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

namespace M_BMilkStoreClient.Pages.ManagementVoucher
{
    public class EditModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        public bool IsAdmin => HttpContext.Session.GetString("UserRole") == "Admin";
        public EditModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (IsAdmin)
            {
                if (id == 0)
                {
                    return NotFound();
                }
                Voucher = await _voucherService.GetVoucherByIdAsync(id);
                if (Voucher == null)
                {
                    return NotFound();
                }
                return Page();
            }
            else
            {
                return Redirect("/Authenticate");
            }
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
                await _voucherService.UpdateVoucherAsync(Voucher);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherExists(Voucher.VoucherId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool VoucherExists(int id)
        {
            return _voucherService.GetVoucherByIdAsync(id) != null; ;
        }
    }
}
