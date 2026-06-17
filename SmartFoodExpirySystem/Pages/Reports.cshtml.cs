using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;

namespace SmartFoodExpirySystem.Pages
{
    public class ReportsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ReportsModel(ApplicationDbContext db) => _db = db;

        public int Total, Expired;
        public decimal WastePercent, TotalValue;
        public List<MonthlySummaryRow> MonthlySummary { get; set; } = new();

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var items = _db.FoodItems
                .Where(f => f.UserId == userId.Value && f.Status != "Used")
                .ToList();

            Total = items.Count;
            Expired = items.Count(f => f.ExpiryDate.Date < DateTime.Today);
            WastePercent = Total > 0 ? Math.Round((decimal)Expired / Total * 100, 1) : 0;
            TotalValue = items.Sum(f => f.Price);

            MonthlySummary = items
                .GroupBy(f => f.PurchaseDate.ToString("MMM yyyy"))
                .Select(g => new MonthlySummaryRow
                {
                    Month = g.Key,
                    Added = g.Count(),
                    Expired = g.Count(f => f.ExpiryDate.Date < DateTime.Today)
                })
                .OrderByDescending(r => r.Month)
                .ToList();

            return Page();
        }

        public class MonthlySummaryRow
        {
            public string Month { get; set; } = "";
            public int Added { get; set; }
            public int Expired { get; set; }
        }
    }
}