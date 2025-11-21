using System.Globalization;
using System.Text.Json;

namespace JobSearch.Services
{
    public class LocalGeoLocationService : IGeoLocationService
    {
        private readonly List<GeoLocationSuggestion> _locations;

        private class RawLocation
        {
            public string name { get; set; } = string.Empty;
        }

        public LocalGeoLocationService()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "polish-locations.json");

            if (!File.Exists(filePath))
            {
                _locations = new List<GeoLocationSuggestion>();
                Console.WriteLine($"[LocalGeoLocationService] Brak pliku: {filePath}");
                return;
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var raw = JsonSerializer.Deserialize<List<RawLocation>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                          ?? new List<RawLocation>();

                _locations = raw
                    .Select(r => new GeoLocationSuggestion
                    {
                        DisplayName = r.name
                    })
                    .DistinctBy(l => l.DisplayName)
                    .OrderBy(l => l.DisplayName)
                    .ToList();

                Console.WriteLine($"[LocalGeoLocationService] Wczytano miejscowości: {_locations.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LocalGeoLocationService] Błąd wczytywania lokalizacji: {ex}");
                _locations = new List<GeoLocationSuggestion>();
            }
        }

        public Task<List<GeoLocationSuggestion>> SuggestLocationsAsync(string term, int maxResults = 20)
        {
            if (string.IsNullOrWhiteSpace(term) || _locations.Count == 0)
                return Task.FromResult(new List<GeoLocationSuggestion>());

            term = term.Trim();

            var results = _locations
                .Where(l => l.DisplayName.StartsWith(term, true, CultureInfo.InvariantCulture))
                .Take(maxResults)
                .ToList();

            return Task.FromResult(results);
        }

        public Task<GeoLocationSuggestion?> GeocodeAsync(string placeName)
        {
            if (string.IsNullOrWhiteSpace(placeName) || _locations.Count == 0)
                return Task.FromResult<GeoLocationSuggestion?>(null);

            var result = _locations.FirstOrDefault(l =>
                string.Equals(l.DisplayName, placeName, StringComparison.InvariantCultureIgnoreCase));

            result ??= _locations.FirstOrDefault(l =>
                l.DisplayName.StartsWith(placeName, true, CultureInfo.InvariantCulture));

            return Task.FromResult(result);
        }
    }
}
