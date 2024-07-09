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

namespace M_BMilkStoreClient.Pages.ManagementOrder
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderService _orderService;
        public bool IsStaff => HttpContext.Session.GetString("UserRole") == "Staff";
        public DetailsModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (IsStaff)
            {
                if (id == 0)
                {
                    return NotFound();
                }
                Order = await _orderService.GetOrderByIdAsync(id);
                if (Order == null)
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
    }
}
