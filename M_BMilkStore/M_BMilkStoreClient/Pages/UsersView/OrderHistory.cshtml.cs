using BussinessObject;
using DataAccessLayer.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.UsersView
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderService _orderService;
        public OrderHistoryModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IList<Order> Orders { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Authenticate"); // Redirect to login if UserId is not found in session
            }
            Orders = await _orderService.GetOrderHistoryByUserIdAsync(userId.Value);
            return Page();
        }
    }
}
