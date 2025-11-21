using JobSearch.Data;
using JobSearch.Services;

public interface IJobOfferService
{
    // --- Istniejące metody (z Twoimi rozszerzonymi filtrami) ---
    Task<List<JobOffer>> GetJobOffersAsync(string? location, decimal? minSalary, string? sortBy, EmploymentType? employmentType, JobType? jobType, IndustryCategory? industryCategory, bool onlyWithExperience, bool onlyOnlineRecruitment, bool onlyWithSanitaryBook, bool onlyWithStudentStatus, bool onlyWithDisabilityCertificate);

    Task<JobOffer?> GetByIdAsync(int id);
    Task CreateAsync(JobOffer jobOffer);
    Task UpdateAsync(JobOffer jobOffer);
    Task DeleteAsync(int id);

    Task<List<JobOffer>> GetMyOffersAsync(string userId);
    Task<bool> ApplyForOfferAsync(int offerId, string applicantId);
    Task<List<JobApplication>> GetApplicationsForOfferAsync(int offerId);
    Task<bool> HasUserAppliedAsync(int offerId, string applicantId);

    Task ApproveOfferAsync(int offerId);
    Task RejectOfferAsync(int offerId);

    Task<List<JobApplication>> GetMyApplicationsAsync(string applicantId);

    // --- ZMIANA SYGNATURY (Zadanie 1): Dodano rejectionReason ---
    Task UpdateApplicationStatusAsync(int applicationId, ApplicationStatus newStatus, string ownerUserId, string? employerPhone = null, string? rejectionReason = null);

    // --- NOWA METODA (Zadanie 3): Przedłużanie oferty ---
    Task ExtendOfferAsync(int offerId, int daysToAdd, string userId);

    Task<JobApplication?> GetApplicationByIdAsync(int applicationId);
}