using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _roleService;
        public EditUserModel(IUserService userService, IUserRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        [BindProperty]
        public User User { get; set; }
        public string UserRole { get; private set; }
        public SelectList UserRoles { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Admin" && UserRole != null)
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            User = await _userService.GetUserByID(id);

            if (User == null)
            {
                return NotFound();
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

            var existingUser = await _userService.GetUserByID(User.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.LastName= User.LastName;
            existingUser.FirstName= User.FirstName;
            existingUser.Name = User.Name;
            existingUser.Email = User.Email;
            existingUser.PhoneNumber = User.PhoneNumber;
            existingUser.RoleId = User.RoleId;
            existingUser.Status = User.Status;
            existingUser.IsDeleted = User.IsDeleted;

            await _userService.UpdateUserAsync(User);
            return RedirectToPage("/Admin/AdminPage");
        }
    }
}
