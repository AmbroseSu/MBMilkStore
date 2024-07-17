using BussinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public int PageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public string UserRole { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
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


            PageIndex = pageIndex ?? 1;

            var pagedResult = await _userService.GetUsersPagedAsync(PageIndex, PageSize, SearchString);

            Users = pagedResult.Items;
            TotalItems = pagedResult.TotalItems;
            TotalPages = pagedResult.TotalPages;

            return Page();
        }
    }
}
