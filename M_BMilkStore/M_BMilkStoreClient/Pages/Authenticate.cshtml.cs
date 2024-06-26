using BussinessObject;
using M_BMilkStoreClient.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace M_BMilkStoreClient.Pages
{
    public class AuthenticateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticateModel> _logger;

        public AuthenticateModel(IUserService userService, ILogger<AuthenticateModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [BindProperty]
        public LoginInputModel LoginInput { get; set; }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        [BindProperty]
        public RegisterInputModel RegisterInput { get; set; }

        public class RegisterInputModel
        {
            [Required(ErrorMessage = "Name is required")]
            [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
            [PasswordPolicy]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Confirm Password is required")]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            ModelState.Clear();

            TryValidateModel(LoginInput, nameof(LoginInput));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetAnUserByEmail(LoginInput.Email);
            if (user != null)
            {
                if (user.Status == false)
                {
                    ViewData["err_msg"] = "Your account has been locked!";
                    return Page();
                }

                if (user.Password == LoginInput.Password && user.Status == true && user.IsDeleted == false && user.RoleId != 1)
                {
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserRole", user.UserRole.UserRoleName);
                    return RedirectToPage("/Index");
                }
                if (user.Password == LoginInput.Password && user.Status == true && user.IsDeleted == false && user.RoleId == 1)
                {
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserRole", user.UserRole.UserRoleName);
                    return RedirectToPage("/Admin/AdminPage");
                }
            }

            ViewData["err_msg"] = "Incorrect Username or Password!";
            return Page();
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            ModelState.Clear();

            TryValidateModel(RegisterInput, nameof(RegisterInput));

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, validationErrors = GetValidationErrors() });
            }

            var userExist = await _userService.GetAnUserByEmail(RegisterInput.Email);
            if (userExist != null)
            {
                return new JsonResult(new { success = false, errorMessage = "Registration failed. User with this email may already exist!" });
            }

            var user = new User
            {
                Name = RegisterInput.Name,
                Email = RegisterInput.Email,
                Password = RegisterInput.Password
            };

            var result = await _userService.CreateUserAsync(user);

            if (result)
            {
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false, errorMessage = "Registration failed. Please try again." });
        }

        private IDictionary<string, string> GetValidationErrors()
        {
            return ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.First().ErrorMessage
                );
        }
    }
}
