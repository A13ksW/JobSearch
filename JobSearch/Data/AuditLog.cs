using JobSearch.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Services
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = string.Empty; // Naprawiono CS8618

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; } // Naprawiono CS8618

        [Required, StringLength(100)]
        public string ActionType { get; set; } = string.Empty; // Naprawiono CS8618

        [Required, StringLength(100)]
        public string EntityType { get; set; } = "JobOffer";

        [Required]
        public int EntityId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Changes { get; set; }
    }
}