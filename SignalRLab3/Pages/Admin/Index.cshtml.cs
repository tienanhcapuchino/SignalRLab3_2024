using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRLab3.DataAccess;
using SignalRLab3.Entities;
using SignalRLab3.Helper;

namespace SignalRLab3.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly SignalRLab3DbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public IndexModel(SignalRLab3DbContext context,
            IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        [BindProperty]
        public List<User> Users { get; set; }
        [BindProperty]
        public List<Group> Groups { get; set; }
        [BindProperty]
        public string Tab { get; set; }

        public IActionResult OnGet(string tab = "user")
        {
            var userLogin = _contextAccessor?.HttpContext?.Session.GetString("user");

            if (string.IsNullOrEmpty(userLogin))
            {
                return RedirectToPage("./Login");
            }

            Tab = tab;
            Users = _context.Users.Include(u => u.Group).ToList();
            Groups = _context.Groups.Include(g => g.Users).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateGroupAsync(Group gr)
        {

            try
            {
                var isExitGroup = await _context.Groups.AnyAsync(g => g.Name == gr.Name);
                if (isExitGroup)
                {
                    return RedirectToPage(new { tab = "group" });
                }

                await _context.Groups.AddAsync(gr);
                await _context.SaveChangesAsync();
                return RedirectToPage(new { tab = "group" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostCreateUserAsync(User user)
        {
            try
            {
                var isExistEmail = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (isExistEmail)
                {
                    return RedirectToPage();
                }

                user.Password = PasswordHelper.Encrypt(user.Password);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
