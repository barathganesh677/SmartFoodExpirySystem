using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public RegisterModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public RegisterInput Input { get; set; } = new();

        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Input.Username) ||
                string.IsNullOrEmpty(Input.Email) ||
                string.IsNullOrEmpty(Input.Password))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            if (_db.Users.Any(u => u.Email == Input.Email))
            {
                ErrorMessage = "Email already registered.";
                return Page();
            }

            var user = new User
            {
                Username = Input.Username,
                Email = Input.Email,
                Password = Input.Password,
                CreatedDate = DateTime.Now
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToPage("/Login");
        }

        public class RegisterInput
        {
            public string Username { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}