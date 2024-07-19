using BussinessObject;
using DataAccessLayer.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.UsersView
{
    public class VouchersModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        public VouchersModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        public List<Voucher> Vouchers { get; set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Customer" && UserRole != null)
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            Vouchers = await VoucherDAO.Instance.GetAllCurrentVouchersAsync();
            return Page();
        }
    }
}
