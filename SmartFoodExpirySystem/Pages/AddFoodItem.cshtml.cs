using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Pages
{
    public class AddFoodItemModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public AddFoodItemModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public FoodItemInput Input { get; set; } = new();

        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                ErrorMessage = "Session expired. Please login again.";
                return Page();
            }

            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var expiry = DateOnly.FromDateTime(Input.ExpiryDate);
                string status = expiry < today ? "Expired"
                              : expiry == today ? "Expiring Today"
                              : "Available";

                var item = new FoodItem
                {
                    ItemName = Input.ItemName,
                    Quantity = Input.Quantity,
                    Price = Input.Price,
                    PurchaseDate = Input.PurchaseDate,
                    ExpiryDate = Input.ExpiryDate,
                    Status = status,
                    Category = Input.Category,   // NEW
                    UserId = userId.Value
                };

                _db.FoodItems.Add(item);
                int rows = _db.SaveChanges();

                Success = rows > 0;
                if (!Success)
                    ErrorMessage = "Save returned 0 rows affected.";

                if (Success)
                {
                    ModelState.Clear();
                    Input = new FoodItemInput();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }

            return Page();
        }

        public class FoodItemInput
        {
            public string ItemName { get; set; } = "";
            public string Quantity { get; set; } = "";
            public decimal Price { get; set; }
            public DateTime PurchaseDate { get; set; } = DateTime.Today;
            public DateTime ExpiryDate { get; set; } = DateTime.Today.AddDays(7);
            public string Category { get; set; } = "Other";   // NEW
        }
    }
}