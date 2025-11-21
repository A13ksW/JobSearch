using JobSearch.Data;
using Microsoft.EntityFrameworkCore;

namespace JobSearch.Services
{
    // Serwis musi być Singletonem, aby utrzymać listę subskrybentów (OnChange)
    public class NotificationService : INotificationService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        // To jest nasz "event", który powiadomi UI (np. NavMenu) o zmianie
        public event Action? OnChange;

        public NotificationService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateNotificationAsync(string userId, string message, string linkUrl)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                LinkUrl = linkUrl,
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            // Po zapisaniu do bazy, uruchom event, aby powiadomić UI
            NotifyStateChanged();
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId, bool includeRead = false)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = context.Notifications
                .Where(n => n.UserId == userId);

            if (!includeRead)
            {
                query = query.Where(n => n.IsRead == false);
            }

            return await query
                .OrderByDescending(n => n.CreatedDate)
                .Take(20) // Ograniczamy do 20 ostatnich
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Notifications
                .CountAsync(n => n.UserId == userId && n.IsRead == false);
        }

        public async Task MarkAsReadAsync(string userId, int? notificationId = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.Notifications
                .Where(n => n.UserId == userId && n.IsRead == false);

            // Jeśli podano ID, oznacz tylko to jedno
            if (notificationId.HasValue)
            {
                query = query.Where(n => n.Id == notificationId.Value);
            }
            // Jeśli nie podano ID, oznacz wszystkie jako przeczytane

            await query.ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));

            // Poinformuj UI, że licznik się zmienił
            NotifyStateChanged();
        }

        // Ta metoda jest wywoływana, aby uruchomić event OnChange
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}