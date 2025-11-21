using System.Text.Json.Serialization;

namespace JobSearch.Services
{
    // Model odpowiedzi z serwisu (to, co zwrócimy do naszego UI)
    public class NipValidationResponse
    {
        public bool IsSuccess { get; set; }
        public string? CompanyName { get; set; }
        public string? ErrorMessage { get; set; }
    }

    // --- Modele do deserializacji odpowiedzi z API (Biała Lista) ---
    // Reprezentują strukturę JSON: { "result": { "subject": { ... } } }

    public class NipSearchResponse
    {
        [JsonPropertyName("result")]
        public NipResultData? Result { get; set; }
    }

    public class NipResultData
    {
        [JsonPropertyName("subject")]
        public NipSubjectData? Subject { get; set; }
    }

    public class NipSubjectData
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; } // Nazwa firmy

        [JsonPropertyName("nip")]
        public string? Nip { get; set; }

        [JsonPropertyName("statusVat")]
        public string? StatusVat { get; set; } // Oczekujemy "Czynny"
    }
}