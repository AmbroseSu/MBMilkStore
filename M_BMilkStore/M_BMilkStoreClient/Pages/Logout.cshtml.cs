using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace M_BMilkStoreClient.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {

            HttpContext.Session.Clear();


            return RedirectToPage("/Authenticate");
        }
    }
}
