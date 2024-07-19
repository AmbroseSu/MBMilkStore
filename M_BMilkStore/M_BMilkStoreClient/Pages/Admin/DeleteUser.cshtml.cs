using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.Admin
{
    public class DeleteUserModel : PageModel
    {
        private readonly IUserService _userService;
        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; }
        public string UserRole { get; private set; }
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

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userService.GetUserByID(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);

            return RedirectToPage("/Admin/AdminPage");
        }
    }
}
