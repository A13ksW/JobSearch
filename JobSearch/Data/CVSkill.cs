using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Data
{
    public class CVSkill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileCVId { get; set; } // Klucz obcy

        [ForeignKey("UserProfileCVId")]
        public virtual UserProfileCV? UserProfileCV { get; set; }

        [Required, StringLength(100)]
        public string SkillName { get; set; } = string.Empty; // Np. "C#", "SQL", "Scrum"
    }
}