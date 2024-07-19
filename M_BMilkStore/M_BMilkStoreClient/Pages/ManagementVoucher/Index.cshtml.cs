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
using DataAccessLayer.DAO;

namespace M_BMilkStoreClient.Pages.ManagementVoucher
{
    public class IndexModel : PageModel
    {

        private readonly IVoucherService _voucherService;
        
        public IndexModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public IList<Voucher> Vouchers { get; private set; }
        public int PageNumber { get; private set; } = 1;
        public int CurrentPage { get; set; }
        public int TotalPages { get; private set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Admin")
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            PageNumber = pageNumber;
                int pageSize = 5;
                Vouchers = await VoucherDAO.Instance.GetVouchersAsync(pageNumber, pageSize);

                int totalVouchers = await _voucherService.GetVoucherCountAsync();
                TotalPages = (int)Math.Ceiling(totalVouchers / (double)pageSize);
                CurrentPage = pageNumber;
            return Page();
           
        }
    }
}
