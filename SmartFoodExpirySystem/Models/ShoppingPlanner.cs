using System.ComponentModel.DataAnnotations;

namespace SmartFoodExpirySystem.Models
{
    public class ShoppingPlanner
    {
        [Key]
        public int PlannerId { get; set; }
        public string ItemName { get; set; } = "";
        public string QuantityNeeded { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public int UserId { get; set; }
    }
}