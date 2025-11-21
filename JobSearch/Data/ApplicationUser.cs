using JobSearch.Services;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Name is required.")]
        public string DisplayName { get; set; } = string.Empty;

        [InverseProperty("CreatedByUser")]
        public virtual ICollection<JobOffer> CreatedJobOffers { get; set; } = new List<JobOffer>();

        [InverseProperty("Applicant")]
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        public virtual UserProfileCV? UserProfileCV { get; set; }

        // --- DODAJ TÊ LINIÊ ---
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

		public string AccountType { get; set; } = "Individual";
	}
}