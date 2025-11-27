using JobSearch.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Data
{
    public enum ExperienceYearsRange
    {
        Zero,           
        LessThanOne,    
        OneToThree,     
        ThreePlus       
    }

    public enum EducationStatus
    {
        Student,
        Absolwent,
        Inne
    }

    public enum WorkPreference
    {
        Nie,
        Tak,
        Obojętnie
    }

    public class UserProfileCV
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public ExperienceYearsRange ExperienceYears { get; set; }
        public EducationStatus EducationStatus { get; set; }

        [StringLength(200)]
        public string? FieldOfStudy { get; set; }

        [StringLength(200)]
        public string? University { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? PreferredCategories { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? PreferredDepartments { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? PreferredLocations { get; set; }

        public WorkPreference RemoteWork { get; set; }
        public WorkPreference Relocation { get; set; }

        [StringLength(255)]
        public string? LinkedInUrl { get; set; }

        [StringLength(255)]
        public string? CvFileUrl { get; set; }

        public virtual ICollection<CVLanguage> Languages { get; set; } = new List<CVLanguage>();
        public virtual ICollection<CVSkill> Skills { get; set; } = new List<CVSkill>();
    }
}