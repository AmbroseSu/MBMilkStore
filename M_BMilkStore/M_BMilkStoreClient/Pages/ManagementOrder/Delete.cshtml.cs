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

namespace M_BMilkStoreClient.Pages.ManagementOrder
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderService _orderService;

        public DeleteModel(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [BindProperty]
        public Order Order { get; set; } = default!;
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
            if (id == 0)
            {
                return NotFound();
            }

            Order = await _orderService.GetOrderByIdAsync(id);

            if (Order == null)
            {
                return NotFound();
            }
            // Remove validation for VoucherName if VoucherId is null
            if (Order.VoucherId == null)
            {
                ModelState.Remove("Order.Voucher.VoucherName");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // Remove validation for VoucherName if VoucherId is null
            if (Order.VoucherId == null)
            {
                ModelState.Remove("Order.Voucher.VoucherName");
            }
            var result = await _orderService.SoftDeleteOrderAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToPage("./Index");
        }
    }
}
