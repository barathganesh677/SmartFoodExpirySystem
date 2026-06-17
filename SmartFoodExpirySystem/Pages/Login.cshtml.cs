using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;

namespace SmartFoodExpirySystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public LoginModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public LoginInput Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.Email == Input.Email && u.Password == Input.Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToPage("/Dashboard");
        }

        public class LoginInput
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}