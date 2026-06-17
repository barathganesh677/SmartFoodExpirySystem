using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartFoodExpirySystem.Models
{
    public class FoodItem
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; } = "";
        public string Quantity { get; set; } = "";

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = "Available";
        public string Category { get; set; } = "Other";   // NEW
        public int UserId { get; set; }
    }
}