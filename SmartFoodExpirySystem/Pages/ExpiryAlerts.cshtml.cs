using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Pages
{
    public class ExpiryAlertsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ExpiryAlertsModel(ApplicationDbContext db) => _db = db;

        public List<FoodItem> Expired { get; set; } = new();
        public List<FoodItem> ExpiringSoon { get; set; } = new();

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var today = DateTime.Today;
            var items = _db.FoodItems
                .Where(f => f.UserId == userId.Value && f.Status != "Used")
                .ToList();

            Expired = items.Where(f => f.ExpiryDate.Date < today).ToList();
            ExpiringSoon = items.Where(f => f.ExpiryDate.Date >= today &&
                                            f.ExpiryDate.Date <= today.AddDays(3)).ToList();
            return Page();
        }
    }
}