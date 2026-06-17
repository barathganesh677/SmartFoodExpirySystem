using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Pages
{
    public class InventoryModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public InventoryModel(ApplicationDbContext db) => _db = db;

        public List<FoodItem> Items { get; set; } = new();
        public string SearchTerm { get; set; } = "";
        public string CategoryFilter { get; set; } = "";

        public IActionResult OnGet(string? search, string? category, int? markUsedId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            // Mark item as Used
            if (markUsedId.HasValue)
            {
                var item = _db.FoodItems.Find(markUsedId.Value);
                if (item != null && item.UserId == userId.Value)
                {
                    item.Status = "Used";
                    _db.SaveChanges();
                }
                return RedirectToPage(new { search, category });
            }

            SearchTerm = search ?? "";
            CategoryFilter = category ?? "";

            var query = _db.FoodItems.Where(f => f.UserId == userId.Value
                                               && f.Status != "Used");

            if (!string.IsNullOrWhiteSpace(SearchTerm))
                query = query.Where(f => f.ItemName.Contains(SearchTerm));

            if (!string.IsNullOrWhiteSpace(CategoryFilter))
                query = query.Where(f => f.Category == CategoryFilter);

            Items = query.OrderBy(f => f.ExpiryDate).ToList();
            return Page();
        }
    }
}