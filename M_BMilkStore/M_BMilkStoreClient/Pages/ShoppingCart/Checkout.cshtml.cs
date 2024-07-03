using BussinessObject;
using M_BMilkStoreClient.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace M_BMilkStoreClient.Pages.ShoppingCart
{
    public class CheckoutModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductLineService iProductLineService;
        public CheckoutModel(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
            iProductLineService = new ProductLineService();
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
                            // Kiểm tra và trừ số lượng sản phẩm trong cart trước khi tạo order
                            bool isQuantityDeducted = await CheckAndDeductProductQuantity(cartItems);

                            if (isQuantityDeducted)
                            {
                                // Tạo danh sách chi tiết đơn hàng
                                var orderDetails = cartItems.Select(ci => new OrderDetail
                                {
                                    ProductId = ci.Product.ProductId,
                                    ProductQuantity = ci.Quantity,
                                    ProductPrice = ci.Product.Price
                                }).ToList();

                                // Tạo đơn hàng và chi tiết đơn hàng
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
                            else
                            {
                                IsUpdateFailed = true; // Đánh dấu lỗi nếu không đủ số lượng sản phẩm
                            }
                        }
                        else
                        {
                            IsUpdateFailed = true; // Đánh dấu lỗi nếu không có sản phẩm trong cart
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
                HttpContext.Session.Remove("DeliveryOptionValue");
                HttpContext.Session.Remove("DeliveryOptionName");
                HttpContext.Session.Remove("TotalPrice");
                return RedirectToPage("/Index");
            }
            return Page();
        }



        private async Task<bool> CheckAndDeductProductQuantity(List<CartItem> cartItems)
        {
            foreach (var cartItem in cartItems)
            {
                var productId = cartItem.Product.ProductId;
                var requestedQuantity = cartItem.Quantity;

                var validProductLines = await iProductLineService.GetProductLinesByProductId(productId);
                       

                int remainingQuantity = requestedQuantity; 
                bool allProductsDeducted = true; 

                foreach (var productLine in validProductLines)
                {
                    if (remainingQuantity <= 0)
                        break; 

                    int availableQuantity = productLine.Quantity; 

                    if (availableQuantity >= remainingQuantity)
                    {
                        productLine.Quantity -= remainingQuantity;

                        if (productLine.Quantity == 0)
                        {
                            productLine.Status = false; 
                            productLine.IsDeleted = true; 
                        }

                        await iProductLineService.UpdateProductLine(productLine);
                        remainingQuantity = 0; 
                    }
                    else
                    {
                       
                        productLine.Quantity = 0; 

                        productLine.Status = false;
                        productLine.IsDeleted = true;

                        await iProductLineService.UpdateProductLine(productLine);
                        remainingQuantity -= availableQuantity; 

                        allProductsDeducted = false; 
                    }
                }

                if(remainingQuantity == 0)
                {
                    return true;
                }
                else
                {
                    if (remainingQuantity > 0 && !allProductsDeducted)
                    {
                        return false;
                    }
                        
                }
                 
            }

            return true; 
        }
    }
}
