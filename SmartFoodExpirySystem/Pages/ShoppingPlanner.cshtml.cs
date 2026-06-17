using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Pages
{
    public class ShoppingPlannerModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ShoppingPlannerModel(ApplicationDbContext db) => _db = db;

        [BindProperty] public string NewItem { get; set; } = "";
        [BindProperty] public string NewQty { get; set; } = "";
        public List<ShoppingPlanner> PlannerItems { get; set; } = new();

        public IActionResult OnGet(int? markId, int? deleteId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            if (markId.HasValue)
            {
                var item = _db.ShoppingPlanners.Find(markId.Value);
                if (item != null) { item.Status = "Purchased"; _db.SaveChanges(); }
            }
            if (deleteId.HasValue)
            {
                var item = _db.ShoppingPlanners.Find(deleteId.Value);
                if (item != null) { _db.ShoppingPlanners.Remove(item); _db.SaveChanges(); }
            }

            PlannerItems = _db.ShoppingPlanners
                              .Where(s => s.UserId == userId.Value)
                              .OrderBy(s => s.Status).ToList();
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            _db.ShoppingPlanners.Add(new ShoppingPlanner
            {
                ItemName = NewItem,
                QuantityNeeded = NewQty,
                Status = "Pending",
                UserId = userId.Value
            });
            _db.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostAutoGenerate()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var expired = _db.FoodItems
                .Where(f => f.UserId == userId.Value && f.ExpiryDate.Date < DateTime.Today
                            && f.Status != "Used")
                .ToList();

            var existing = _db.ShoppingPlanners
                .Where(s => s.UserId == userId.Value && s.Status == "Pending")
                .Select(s => s.ItemName.ToLower())
                .ToHashSet();

            foreach (var item in expired.Where(e => !existing.Contains(e.ItemName.ToLower())))
            {
                _db.ShoppingPlanners.Add(new ShoppingPlanner
                {
                    ItemName = item.ItemName,
                    QuantityNeeded = item.Quantity,
                    Status = "Pending",
                    UserId = userId.Value
                });
                existing.Add(item.ItemName.ToLower());  // prevent dup within same batch too
            }
            _db.SaveChanges();
            return RedirectToPage();
        }
    }
}