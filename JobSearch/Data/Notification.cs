using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Data
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        // Do kogo należy to powiadomienie
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Link, na który przejdzie użytkownik po kliknięciu
        [StringLength(255)]
        public string? LinkUrl { get; set; }
    }
}