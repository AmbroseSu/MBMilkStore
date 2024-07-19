using System.ComponentModel.DataAnnotations;
using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using M_BMilkStoreClient.Service;
using Service;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.ShoppingCart
{
    public class CheckoutModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductLineService iProductLineService;
        private readonly IVoucherService _voucherService;

        public CheckoutModel(
            IUserService userService,
            IOrderService orderService,
            IVoucherService voucherService
        )
        {
            _userService = userService;
            _orderService = orderService;
            iProductLineService = new ProductLineService();
            _voucherService = voucherService;
        }

        [TempData]
        public string ToastMessage { get; set; }

        [TempData]
        public string ToastType { get; set; } // "success" or "error"
        public string? TotalPrice { get; private set; }

        [BindProperty]
        public string? DeliveryOptionName { get; set; }

        [BindProperty]
        public float DeliveryOptionValue { get; set; }

        public float FinalPrice
        {
            get
            {
                var totalPrice = float.Parse(HttpContext.Session.GetString("TotalPrice") ?? "0.00");
                var deliveryValue = float.Parse(
                    HttpContext.Session.GetString("DeliveryOptionValue") ?? "5.00"
                );
                var voucherValue = float.Parse(
                    HttpContext.Session.GetString("VoucherValue") ?? "0.00"
                );
                return totalPrice + deliveryValue - voucherValue;
            }
        }

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

        [BindProperty]
        public string? VoucherCode { get; set; }
        public float VoucherValue { get; private set; }
        public int VoucherId { get; private set; }
        public bool IsUpdateSuccessful { get; private set; }
        public bool IsUpdateFailed { get; private set; }
        public bool IsVoucherApplied { get; private set; }
        public string? VoucherMessage { get; private set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Customer"&&UserRole!=null)
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            TotalPrice = HttpContext.Session.GetString("TotalPrice") ?? "0.00";
            DeliveryOptionName =
                HttpContext.Session.GetString("DeliveryOptionName") ?? "Standard Delivery";
            DeliveryOptionValue = float.Parse(
                HttpContext.Session.GetString("DeliveryOptionValue") ?? "5.00"
            );
            VoucherValue = float.Parse(HttpContext.Session.GetString("VoucherValue") ?? "0.00");
            VoucherCode = HttpContext.Session.GetString("VoucherName");
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
            if (TempData.ContainsKey("IsVoucherApplied"))
            {
                IsVoucherApplied = (bool)TempData["IsVoucherApplied"];
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TotalPrice = HttpContext.Session.GetString("TotalPrice") ?? TotalPrice;
            DeliveryOptionValue = float.Parse(
                HttpContext.Session.GetString("DeliveryOptionValue") ?? "5.00"
            );
            VoucherValue = float.Parse(HttpContext.Session.GetString("VoucherValue") ?? "0.00");
            float finalPrice = (
                float.Parse(TotalPrice ?? "0.00") + DeliveryOptionValue - VoucherValue
            );
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
                        var cartItems = SessionService.GetSessionObjectAsJson<List<CartItem>>(
                            HttpContext.Session,
                            "cart"
                        );
                        if (cartItems != null && cartItems.Count > 0)
                        {
                            // Kiểm tra và trừ số lượng sản phẩm trong cart trước khi tạo order
                            bool isQuantityDeducted = await CheckAndDeductProductQuantity(
                                cartItems
                            );

                            if (isQuantityDeducted)
                            {
                                // Tạo danh sách chi tiết đơn hàng
                                var orderDetails = cartItems
                                    .Select(ci => new OrderDetail
                                    {
                                        ProductId = ci.Product.ProductId,
                                        ProductQuantity = ci.Quantity,
                                        ProductPrice = ci.Product.Price
                                    })
                                    .ToList();

                                // Tạo đơn hàng và chi tiết đơn hàng
                                if (float.TryParse(TotalPrice, out float parsedTotalPrice))
                                {
                                    int? voucherId = null;

                                    string voucherIdStr = HttpContext.Session.GetString(
                                        "VoucherID"
                                    );
                                    if (
                                        !string.IsNullOrEmpty(voucherIdStr)
                                        && int.TryParse(voucherIdStr, out int parsedVoucherId)
                                    )
                                    {
                                        voucherId = parsedVoucherId;
                                    }

                                    int orderId = await _orderService.CreateOrderAsync(
                                        userId.Value,
                                        finalPrice,
                                        voucherId
                                    );
                                    bool isOrderDetailsCreated =
                                        await _orderService.CreateOrderDetailsAsync(
                                            orderId,
                                            orderDetails
                                        );
                                    IsUpdateSuccessful = isOrderDetailsCreated;
                                    ToastMessage = "Order created successfully!";
                                    ToastType = "success";
                                    // Call ClaimVoucherAsync if voucher is used
                                    if (voucherId.HasValue)
                                    {
                                        await _voucherService.ClaimVoucherAsync(
                                            userId.Value,
                                            voucherId.Value
                                        );
                                    }
                                }
                                else
                                {
                                    IsUpdateFailed = true;
                                    ToastMessage = "Failed to create order!";
                                    ToastType = "error";
                                }
                            }
                            else
                            {
                                IsUpdateFailed = true; // Đánh dấu lỗi nếu không đủ số lượng sản phẩm
                                ToastMessage = "Insufficient product quantity!";
                                ToastType = "error";
                            }
                        }
                        else
                        {
                            IsUpdateFailed = true; // Đánh dấu lỗi nếu không có sản phẩm trong cart
                            ToastMessage = "No items in the cart!";
                            ToastType = "error";
                        }
                    }
                    catch
                    {
                        IsUpdateFailed = true;
                        ToastMessage = "An error occurred while updating!";
                        ToastType = "error";
                    }
                }
            }

            if (IsUpdateSuccessful)
            {
                HttpContext.Session.Remove("cart");
                HttpContext.Session.Remove("DeliveryOptionValue");
                HttpContext.Session.Remove("DeliveryOptionName");
                HttpContext.Session.Remove("TotalPrice");
                HttpContext.Session.Remove("VoucherID");
                HttpContext.Session.Remove("VoucherValue");
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostApplyVoucherAsync()
        {
            if (string.IsNullOrEmpty(VoucherCode))
            {
                ModelState.AddModelError(string.Empty, "Voucher code is required.");
                return Page();
            }

            if (!decimal.TryParse(FinalPrice.ToString(), out decimal currentFinalPrice))
            {
                ModelState.AddModelError(string.Empty, "Invalid total price.");
                return Page();
            }

            // Get the user ID from the session
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                ToastMessage = "User not logged in.";
                ToastType = "error";
                return Page();
            }
            var result = await ApplyVoucherAsync(VoucherCode, currentFinalPrice, userId.Value);
            if (!result.IsValid)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                ToastMessage = result.ErrorMessage;
                ToastType = "error";
            }
            else
            {
                // Save voucher details to session
                HttpContext.Session.SetString("VoucherID", result.VoucherId.ToString());
                HttpContext.Session.SetString("VoucherValue", result.VoucherValue.ToString());
                HttpContext.Session.SetString("VoucherName", result.VoucherName.ToString());
                IsVoucherApplied = true;
                VoucherMessage = "Voucher applied successfully!";
                ToastMessage = "Voucher applied successfully!";
                ToastType = "success";
                TempData["IsVoucherApplied"] = true;
            }

            return RedirectToPage();
        }

        private async Task<(
            bool IsValid,
            int VoucherId,
            decimal VoucherValue,
            string VoucherName,
            string ErrorMessage
        )> ApplyVoucherAsync(string voucherCode, decimal currentFinalPrice, int userId)
        {
            var isValidVoucher = await _voucherService.IsValidVoucherAsync(
                voucherCode,
                currentFinalPrice
            );
            if (!isValidVoucher)
            {
                return (false, 0, 0, "None", "Invalid or expired voucher code.");
            }

            var voucher = await _voucherService.GetVoucherByCodeAsync(voucherCode);
            if (voucher == null)
            {
                return (false, 0, 0, "None", "Voucher not found.");
            }

            // Check if the user has already claimed the voucher
            bool hasClaimed = await _voucherService.HasUserAlreadyClaimedVoucherAsync(
                userId,
                voucher.VoucherId
            );
            if (hasClaimed)
            {
                return (false, 0, 0, "None", "You have already claimed this voucher.");
            }

            return (
                true,
                voucher.VoucherId,
                voucher.VoucherValue,
                voucher.VoucherName,
                string.Empty
            );
        }

        /*        private async Task<bool> CheckAndDeductProductQuantity(List<CartItem> cartItems)
                {
                    foreach (var cartItem in cartItems)
                    {
                        var productId = cartItem.Product.ProductId;
                        var requestedQuantity = cartItem.Quantity;

                        var validProductLines = await iProductLineService.GetProductLinesByProductId(
                            productId
                        );

                        int remainingQuantity = requestedQuantity;
                        bool allProductsDeducted = true;

                        foreach (var productLine in validProductLines)
                        {
                            if (remainingQuantity <= 0)
                                break;

                            int availableQuantity = productLine.QuantityOut;

                            if (availableQuantity >= remainingQuantity)
                            {
                                productLine.QuantityOut -= remainingQuantity;

                                if (productLine.QuantityOut == 0)
                                {
                                    productLine.Status = false;
                                    productLine.IsDeleted = true;
                                }

                                await iProductLineService.UpdateProductLine(productLine);
                                remainingQuantity = 0;
                            }
                            else
                            {
                                productLine.QuantityOut = 0;

                                productLine.Status = false;
                                productLine.IsDeleted = true;

                                await iProductLineService.UpdateProductLine(productLine);
                                remainingQuantity -= availableQuantity;

                                allProductsDeducted = false;
                            }
                        }

                        if (remainingQuantity == 0)
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
                }*/

        private async Task<bool> CheckAndDeductProductQuantity(List<CartItem> cartItems)
        {
            foreach (var cartItem in cartItems)
            {
                var productId = cartItem.Product.ProductId;
                var requestedQuantity = cartItem.Quantity;

                var validProductLines = await iProductLineService.GetProductLinesByProductId(productId);

                int remainingQuantity = requestedQuantity;

                foreach (var productLine in validProductLines)
                {
                    if (remainingQuantity <= 0)
                        break;

                    int availableQuantity = productLine.QuantityOut;

                    if (availableQuantity >= remainingQuantity)
                    {
                        productLine.QuantityOut -= remainingQuantity;

                        if (productLine.QuantityOut == 0)
                        {
                            productLine.Status = false;
                            productLine.IsDeleted = true;
                        }

                        await iProductLineService.UpdateProductLine(productLine);
                        remainingQuantity = 0;
                    }
                    else
                    {
                        productLine.QuantityOut = 0;
                        productLine.Status = false;
                        productLine.IsDeleted = true;
                        await iProductLineService.UpdateProductLine(productLine);
                        remainingQuantity -= availableQuantity;
                    }
                }

                if (remainingQuantity > 0)
                {
                    // Nếu không trừ đủ số lượng cho sản phẩm này, trả về false và không cần kiểm tra các sản phẩm khác
                    return false;
                }
            }

            // Nếu tất cả sản phẩm đều trừ đủ số lượng, trả về true
            return true;
        }


    }

}
