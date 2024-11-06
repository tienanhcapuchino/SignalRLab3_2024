using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRLab3.Pages.Admin
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LogoutModel(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public IActionResult OnGet()
        {
            _contextAccessor.HttpContext.Session.Clear();
            return RedirectToPage("./Login");
        }

    }
}
