using System.ComponentModel.DataAnnotations;

namespace Amazon_Replica.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quatity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
