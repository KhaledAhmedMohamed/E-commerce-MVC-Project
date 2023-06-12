using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Amazon_Replica.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthdate { get; set; }

        public string? Address { get; set; }

    }
}
