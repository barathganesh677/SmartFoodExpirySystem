using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DashboardModel(ApplicationDbContext db) => _db = db;

        public int TotalItems, AvailableItems, ExpiredItems, ExpiringItems;
        public List<FoodItem> ExpiringList { get; set; } = new();
        public List<ShoppingPlanner> ShoppingList { get; set; } = new();
        public List<FoodItem> ExpiringTomorrow { get; set; } = new();

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            // Exclude "Used" items from all dashboard stats
            var items = _db.FoodItems
                .Where(f => f.UserId == userId.Value && f.Status != "Used")
                .ToList();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            TotalItems = items.Count;
            ExpiredItems = items.Count(f => f.ExpiryDate.Date < today);
            ExpiringItems = items.Count(f => f.ExpiryDate.Date >= today &&
                                              f.ExpiryDate.Date <= today.AddDays(3));
            AvailableItems = items.Count(f => f.ExpiryDate.Date > today.AddDays(3));

            ExpiringList = items
                .Where(f => f.ExpiryDate.Date >= today && f.ExpiryDate.Date <= today.AddDays(3))
                .OrderBy(f => f.ExpiryDate).ToList();

            ExpiringTomorrow = items
                .Where(f => f.ExpiryDate.Date == tomorrow)
                .ToList();

            ShoppingList = _db.ShoppingPlanners
                .Where(s => s.UserId == userId.Value)
                .Take(5).ToList();

            return Page();
        }
    }
}