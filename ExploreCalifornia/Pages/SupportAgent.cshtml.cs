using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExploreCalifornia.Pages
{
    [Authorize]
    public class SupportAgentModel : PageModel
    {

        public void OnGet()
        {

        }
    }
}