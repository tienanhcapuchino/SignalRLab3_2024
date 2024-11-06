using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRLab3.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        public Guid GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
