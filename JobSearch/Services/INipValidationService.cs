namespace JobSearch.Services
{
    public interface INipValidationService
    {
        // Metoda sprawdzi NIP i zwróci status oraz nazwę firmy, jeśli się powiedzie
        Task<NipValidationResponse> ValidateNipAsync(string nip);
    }
}