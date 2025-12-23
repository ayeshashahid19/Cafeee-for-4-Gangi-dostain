
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string CashierName { get; set; }

        public DateTime OrderDate { get; set; }

        //public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // ✅ ADD THIS
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}


