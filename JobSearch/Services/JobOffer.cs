using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobSearch.Data;
using System.Reflection;

namespace JobSearch.Services
{
    public enum EmploymentType
    {
        [Display(Name = "Pełny etat")] FullTime,
        [Display(Name = "Część etatu")] PartTime,
        [Display(Name = "Kontrakt B2B")] Contract,
        [Display(Name = "Staż / Praktyka")] Internship,
        [Display(Name = "Praca tymczasowa")] Temporary,
        [Display(Name = "Freelance")] Freelance
    }

    public enum JobType
    {
        [Display(Name = "Zdalna")] Remote,
        [Display(Name = "Stacjonarna")] OnSite,
        [Display(Name = "Hybrydowa")] Hybrid
    }

    public enum OfferStatus
    {
        [Display(Name = "Weryfikacja")] Weryfikacja,
        [Display(Name = "Opublikowana")] Opublikowana,
        [Display(Name = "Odrzucona")] Odrzucona,
        [Display(Name = "Wygasła")] Wygasła
    }

    public enum IndustryCategory
    {
        None, Sprzedaz, Marketing, Finanse, Bankowosc, ObslugaKlienta, ZdrowieIUroda, Gastronomia,
        Turystyka, Zarzadzanie, PracaWSklepie, Budownictwo, Produkcja, Nieruchomosci, Edukacja,
        Logistyka, HR, Design, BIData, PracaBiurowa, Consulting, Media, Prawo, IT
    }
    public static class EnumText
    {
        public static string Display(Enum value)
        {
            var members = value.GetType().GetMember(value.ToString());
            if (members.Length == 0) return value.ToString();

            var display = members[0].GetCustomAttribute<DisplayAttribute>();
            return display?.GetName() ?? value.ToString();
        }
    }

    public class JobOffer : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł stanowiska jest wymagany.")]
        [StringLength(80, ErrorMessage = "Tytuł nie może przekraczać 80 znaków.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis stanowiska jest wymagany.")]
        [StringLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwa firmy jest wymagana.")]
        [StringLength(500, ErrorMessage = "Nazwa firmy nie może przekraczać 500 znaków.")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer NIP jest wymagany.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP musi składać się z 10 cyfr.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "NIP musi składać się wyłącznie z 10 cyfr.")]
        public string Nip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Musisz wybrać branżę.")]
        public IndustryCategory? IndustryCategory { get; set; }

        [Required(ErrorMessage = "Lokalizacja jest wymagana.")]
        [StringLength(100, ErrorMessage = "Lokalizacja nie może przekraczać 100 znaków.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email kontaktowy jest wymagany.")]
        [StringLength(250, ErrorMessage = "Email jest za długi.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny format adresu e-mail.")]
        public string ContactInfo { get; set; } = string.Empty;

        [Column(TypeName = "datetime2")]
        public DateTime DatePosted { get; set; }

        [Required(ErrorMessage = "Data ważności ogłoszenia jest wymagana.")]
        public DateTime? ApplicationDeadline { get; set; }

        [Required(ErrorMessage = "Wybierz rodzaj zatrudnienia.")]
        public EmploymentType EmplType { get; set; }

        [Required(ErrorMessage = "Wybierz tryb pracy.")]
        public JobType JobType { get; set; }

        public bool RequiresExperience { get; set; }
        public bool IsOnlineRecruitment { get; set; }
        public bool RequiresSanitaryBook { get; set; }
        public bool RequiresStudentStatus { get; set; }
        public bool RequiresDisabilityCertificate { get; set; }

        [NotMapped]
        public string SalaryRange
        {
            get
            {
                if (SalaryMin.HasValue && SalaryMax.HasValue)
                    return $"{SalaryMin.Value:0.##} - {SalaryMax.Value:0.##}";
                if (SalaryMin.HasValue)
                    return $"{SalaryMin.Value:0.##} - ?";
                if (SalaryMax.HasValue)
                    return $"? - {SalaryMax.Value:0.##}";
                return "Do negocjacji";
            }
        }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalaryMin { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalaryMax { get; set; }

        [Required(ErrorMessage = "Wymagania są obowiązkowe.")]
        [StringLength(500, ErrorMessage = "Wymagania nie mogą przekraczać 500 znaków.")]
        public string Requirements { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Benefity nie mogą przekraczać 200 znaków.")]
        public string? Benefits { get; set; }

        [Required]
        public OfferStatus Status { get; set; } = OfferStatus.Weryfikacja;

        [Column(TypeName = "nvarchar(1000)")]
        public string? ModerationComment { get; set; }

        public string? CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser? CreatedByUser { get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SalaryMin.HasValue && SalaryMax.HasValue && SalaryMin > SalaryMax)
            {
                yield return new ValidationResult("Wynagrodzenie minimalne nie może być wyższe niż maksymalne.",
                    new[] { nameof(SalaryMax) });
            }
        }
    }
}