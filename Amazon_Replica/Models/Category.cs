using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Amazon_Replica.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        public byte[]? Image { get; set; }

        public virtual ICollection<Product>? Products { get; set; } = new HashSet<Product>();
    }
}
