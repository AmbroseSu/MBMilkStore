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

namespace M_BMilkStoreClient.Pages.ManagementVoucher
{
    public class DeleteModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        public bool IsStaff => HttpContext.Session.GetString("UserRole") == "Staff";
        public DeleteModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = default!;
        public string MessageError { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if(IsStaff)
            {

                if (id == 0)
                {
                    return NotFound();
                }

                var voucher = await _voucherService.GetVoucherByIdAsync(id);
                if (voucher == null)
                {
                    return NotFound();
                }
                else
                {
                    Voucher = voucher;
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Authenticate");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!IsStaff)
            {
                return RedirectToPage("/Authenticate");
            }
            try
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var voucher = await _voucherService.GetVoucherByIdAsync(id);
                if (voucher != null)
                {
                    Voucher = voucher;
                    await _voucherService.DeleteVoucherAsync(voucher.VoucherId);
                }
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                MessageError = ex.Message;
                return Page();
            }
        }
    }
}
