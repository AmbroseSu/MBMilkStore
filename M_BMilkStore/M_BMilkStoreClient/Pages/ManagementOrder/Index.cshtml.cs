using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Service.Interfaces;
using DataAccessLayer.DAO;

namespace M_BMilkStoreClient.Pages.ManagementOrder
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private const int PageSize = 5;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IList<Order> Orders { get; set; } = new List<Order>();
        public int CurrentPage { get; private set; } = 1;
        public int TotalPages { get; private set; }

        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
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

            CurrentPage = pageNumber;
            var (orders, totalOrders) = await _orderService.GetOrdersAsync(pageNumber, PageSize);
            TotalPages = (int)Math.Ceiling(totalOrders / (double)PageSize);
            Orders = orders;

            return Page();
        }
    }
}
