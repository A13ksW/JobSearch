using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Data
{
    public class CVLanguage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileCVId { get; set; }

        [ForeignKey("UserProfileCVId")]
        public virtual UserProfileCV? UserProfileCV { get; set; }

        [Required, StringLength(100)]
        public string LanguageName { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string LanguageLevel { get; set; } = string.Empty;
    }
}