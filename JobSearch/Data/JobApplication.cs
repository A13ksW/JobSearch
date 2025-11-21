using JobSearch.Data;
using JobSearch.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Data
{
    public enum ApplicationStatus
    {
        Aplikowano,
        Zaproszony,
        Odrzucony
    }

    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobOfferId { get; set; }

        [ForeignKey("JobOfferId")]
        public virtual JobOffer? JobOffer { get; set; }

        [Required]
        public string ApplicantId { get; set; } = string.Empty;

        [ForeignKey("ApplicantId")]
        public virtual ApplicationUser? Applicant { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Aplikowano;

        [StringLength(50)]
        public string? EmployerContactPhone { get; set; }

        // --- NOWE POLA ---

        // Data ostatniej zmiany statusu (do blokady 30 min)
        public DateTime? StatusLastUpdated { get; set; }

        // Powód odrzucenia (feedback)
        [StringLength(1000)]
        public string? RejectionReason { get; set; }
    }
}