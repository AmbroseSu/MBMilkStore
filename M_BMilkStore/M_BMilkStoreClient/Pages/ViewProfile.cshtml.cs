using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages
{
    public class ViewProfileModel : PageModel
    {
        private readonly IUserService _userService;
        public ViewProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId =HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Authenticate");
            }
            User = await _userService.GetUserByID((int) userId);
            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
