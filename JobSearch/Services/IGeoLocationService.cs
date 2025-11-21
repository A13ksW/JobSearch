using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobSearch.Services
{
    public interface IGeoLocationService
    {
        Task<List<GeoLocationSuggestion>> SuggestLocationsAsync(string term, int maxResults = 10);

      
        Task<GeoLocationSuggestion?> GeocodeAsync(string placeName);
    }
}
