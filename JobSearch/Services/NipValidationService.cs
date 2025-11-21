using System.Net.Http.Json;

namespace JobSearch.Services
{
    public class NipValidationService : INipValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NipValidationService> _logger;

        public NipValidationService(HttpClient httpClient, ILogger<NipValidationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<NipValidationResponse> ValidateNipAsync(string nip)
        {
            // 1. Walidacja formatu (już sprawdzona w modelu, ale warto podwójnie)
            if (string.IsNullOrWhiteSpace(nip) || nip.Length != 10 || !nip.All(char.IsDigit))
            {
                return new NipValidationResponse { IsSuccess = false, ErrorMessage = "NIP musi składać się z 10 cyfr." };
            }

            // 2. Przygotowanie zapytania do API (Biała Lista)
            string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var url = $"api/search/nip/{nip}?date={today}";

            try
            {
                // 3. Wykonanie zapytania
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Błąd API NIP. Status: {response.StatusCode}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return new NipValidationResponse { IsSuccess = false, ErrorMessage = "Nie znaleziono firmy o podanym NIP." };
                    }
                    return new NipValidationResponse { IsSuccess = false, ErrorMessage = "Błąd serwisu weryfikacji NIP." };
                }

                // 4. Deserializacja odpowiedzi
                var apiResponse = await response.Content.ReadFromJsonAsync<NipSearchResponse>();

                if (apiResponse?.Result?.Subject == null)
                {
                    return new NipValidationResponse { IsSuccess = false, ErrorMessage = "Nie znaleziono firmy (pusta odpowiedź)." };
                }

                var subject = apiResponse.Result.Subject;

                // 5. Walidacja biznesowa - czy firma jest aktywnym płatnikiem VAT?
                if (subject.StatusVat != "Czynny")
                {
                    _logger.LogWarning($"NIP {nip} nie jest czynnym podatnikiem VAT (Status: {subject.StatusVat}).");
                    return new NipValidationResponse { IsSuccess = false, ErrorMessage = $"Firma nie jest czynnym podatnikiem VAT (Status: {subject.StatusVat})." };
                }

                // 6. Sukces! Zwracamy nazwę firmy.
                return new NipValidationResponse { IsSuccess = true, CompanyName = subject.Name };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Krytyczny błąd podczas walidacji NIP.");
                return new NipValidationResponse { IsSuccess = false, ErrorMessage = $"Błąd połączenia z API: {ex.Message}" };
            }
        }
    }
}