using JobSearch.Data;

namespace JobSearch.Services
{
    public interface INotificationService
    {
        // Zdarzenie (event) C# do powiadamiania UI o zmianach
        event Action? OnChange;

        // Metoda do wysłania nowego powiadomienia
        Task CreateNotificationAsync(string userId, string message, string linkUrl);

        // Metoda do pobrania powiadomień dla użytkownika
        Task<List<Notification>> GetNotificationsAsync(string userId, bool includeRead = false);

        // Metoda do oznaczenia powiadomień jako przeczytane
        Task MarkAsReadAsync(string userId, int? notificationId = null);

        // Metoda do liczenia nieprzeczytanych powiadomień
        Task<int> GetUnreadCountAsync(string userId);
    }
}