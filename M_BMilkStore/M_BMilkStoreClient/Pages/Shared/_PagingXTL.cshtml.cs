using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace M_BMilkStoreClient.Pages.Shared
{
    public class _PagingXTLModel : PageModel
    {
        public int currentpage { get; set; }
        public int countpages { get; set; }
        public Func<int?, string> generateUrl { get; set; }
        public void OnGet()
        {
        }
    }
}
