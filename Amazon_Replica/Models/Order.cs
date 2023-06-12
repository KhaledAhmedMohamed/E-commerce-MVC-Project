using Amazon_Replica.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Amazon_Replica.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        [DataType(DataType.Currency)]
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
