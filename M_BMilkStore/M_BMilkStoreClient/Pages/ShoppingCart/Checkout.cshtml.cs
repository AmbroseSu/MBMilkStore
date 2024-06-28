using BussinessObject;
using M_BMilkStoreClient.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace M_BMilkStoreClient.Pages.ShoppingCart
{
    public class CheckoutModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public CheckoutModel(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }
        public string? TotalPrice { get; private set; }
        [BindProperty]
        public string? DeliveryOptionName { get; set; }
        [BindProperty]
        public float DeliveryOptionValue { get; set; }

        public float FinalPrice => (float.Parse(TotalPrice ?? "0.00") + DeliveryOptionValue);

        [BindProperty]
        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        public bool IsUpdateSuccessful { get; private set; }
        public bool IsUpdateFailed { get; private set; }
        public async Task OnGetAsync()
        {
            TotalPrice = HttpContext.Session.GetString("TotalPrice") ?? "0.00";
            DeliveryOptionName = HttpContext.Session.GetString("DeliveryOptionName") ?? "Standard Delivery";
            DeliveryOptionValue = float.Parse(HttpContext.Session.GetString("DeliveryOptionValue") ?? "5.00");

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                // Fetch user details using UserService
                var user = await _userService.GetUserByID(userId.Value);

                if (user != null)
                {
                    FirstName = user.FirstName;
                    LastName = user.LastName;
                    Address = user.Address;
                    Email = user.Email;
                    PhoneNumber = user.PhoneNumber;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TotalPrice = HttpContext.Session.GetString("TotalPrice") ?? TotalPrice;
            DeliveryOptionValue = float.Parse(HttpContext.Session.GetString("DeliveryOptionValue") ?? "5.00");
            float finalPrice = (float.Parse(TotalPrice ?? "0.00") + DeliveryOptionValue);
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = await _userService.GetUserByID(userId.Value);
                if (user != null)
                {
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.Address = Address;
                    user.Email = Email;
                    user.PhoneNumber = PhoneNumber;

                    try
                    {
                        await _userService.UpdateUserAsync(user);

                        // Create the order
                        var cartItems = SessionService.GetSessionObjectAsJson<List<CartItem>>(HttpContext.Session, "cart");
                        if (cartItems != null && cartItems.Count > 0)
                        {
                            var orderDetails = cartItems.Select(ci => new OrderDetail
                            {
                                ProductId = ci.Product.ProductId,
                                ProductQuantity = ci.Quantity,
                                ProductPrice = ci.Product.Price
                            }).ToList();

                            if (float.TryParse(TotalPrice, out float parsedTotalPrice))
                            {
                                int orderId = await _orderService.CreateOrderAsync(userId.Value, finalPrice, 1);
                                bool isOrderDetailsCreated = await _orderService.CreateOrderDetailsAsync(orderId, orderDetails);
                                IsUpdateSuccessful = isOrderDetailsCreated;
                            }
                            else
                            {
                                IsUpdateFailed = true;
                            }
                        }
                    }
                    catch
                    {
                        IsUpdateFailed = true;
                    }
                }
            }

            if (IsUpdateSuccessful)
            {
                HttpContext.Session.Remove("cart");
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
