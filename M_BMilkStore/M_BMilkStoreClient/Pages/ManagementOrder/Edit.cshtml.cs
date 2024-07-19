using System;
using System.Threading.Tasks;
using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.ManagementOrder
{
    public class EditModel : PageModel
    {
        private readonly IOrderService _orderService;

        public EditModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int id)
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

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Order.User.Name");
            ModelState.Remove("Order.User.Password");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _orderService.UpdateOrderAsync(Order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(Order.OrderId))
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


        private async Task<bool> OrderExists(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return order != null;
        }
    }
}
