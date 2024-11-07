using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRLab3.DataAccess;
using SignalRLab3.Helper;

namespace SignalRLab3.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly SignalRLab3DbContext _context;

        public LoginModel(IHttpContextAccessor contextAccessor, SignalRLab3DbContext context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (_contextAccessor?.HttpContext?.Session.GetString("user") != null)
            {
                return RedirectToPage("../Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UserLoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null && PasswordHelper.Decrypt(user.Password) == model.Password)
            {
                _contextAccessor.HttpContext.Session.SetString("user", user.Email);
                _contextAccessor.HttpContext.Session.SetString("userId", user.Id.ToString());
                return RedirectToPage("../Index");
            }
            return Page();
        }
        public IActionResult OnGetLogout()
        {
            _contextAccessor.HttpContext.Session.Clear();
            return Page();
        }
    }
}
