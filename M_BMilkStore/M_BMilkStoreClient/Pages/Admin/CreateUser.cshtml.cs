using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.Admin
{
    public class CreateUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _roleService;
        public CreateUserModel(IUserService userService, IUserRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        [BindProperty]
        public User User { get; set; }

        public SelectList UserRoles { get; set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Admin")
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            UserRoles = new SelectList(await _roleService.GetAllUserRolesAsync(), "UserRoleId", "UserRoleName");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                UserRoles = new SelectList(await _roleService.GetAllUserRolesAsync(), "UserRoleId", "UserRoleName");
                return Page();
            }
            var userExisting=await _userService.GetAnUserByEmail(User.Email);
            if(userExisting != null)
            {
                ModelState.AddModelError("User.Email", "Email already exists.");
                UserRoles = new SelectList(await _roleService.GetAllUserRolesAsync(), "UserRoleId", "UserRoleName");
                return Page();
            }

            await _userService.CreateUserAsync(User);
            return RedirectToPage("/Admin/AdminPage");
        }
    }
}
