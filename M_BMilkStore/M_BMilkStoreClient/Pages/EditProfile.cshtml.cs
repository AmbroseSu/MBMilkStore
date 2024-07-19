using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.Threading.Tasks;

namespace M_BMilkStoreClient.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly IUserService _userService;

        public EditProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; }
        public string UserRole { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");
            if (UserRole != "Customer")
            {
                return RedirectToPage("/Error");
            }
            if (UserRole == null)
            {
                return RedirectToPage("/Authenticate");
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Authenticate");
            }

            User = await _userService.GetUserByID((int)userId);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Authenticate");
            }

            var userToUpdate = await _userService.GetUserByID((int)userId);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.LastName = User.LastName;
            userToUpdate.FirstName = User.FirstName;
            userToUpdate.Address = User.Address;
            userToUpdate.Name = User.Name;
            userToUpdate.Email = User.Email;
            userToUpdate.Password = User.Password;
            userToUpdate.PhoneNumber = User.PhoneNumber;

            await _userService.UpdateUserAsync(userToUpdate);

            return RedirectToPage("ViewProfile");
        }
    }
}
