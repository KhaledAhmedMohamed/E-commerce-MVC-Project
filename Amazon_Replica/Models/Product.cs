using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Amazon_Replica.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public int NumInStock { get; set; }

        public byte[]? Image { get; set; }
 
        [ForeignKey("Category")]
        public virtual int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
