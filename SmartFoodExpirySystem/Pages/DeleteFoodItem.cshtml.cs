using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartFoodExpirySystem.Data;

namespace SmartFoodExpirySystem.Pages
{
    public class DeleteFoodItemModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteFoodItemModel(ApplicationDbContext db) => _db = db;

        public IActionResult OnGet(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var item = _db.FoodItems.Find(id);
            if (item != null)
            {
                _db.FoodItems.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToPage("/Inventory");
        }
    }
}