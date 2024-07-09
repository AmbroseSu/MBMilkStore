using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages.Admin
{
    public class AdminPageModel : PageModel
    {
        private readonly IUserService _userService;
        public AdminPageModel(IUserService userService)
        {
            _userService = userService;
        }
        public IList<User> Users { get; set; }
        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllUsersAsync();
        }
    }
}
