using JobSearch.Data;
using Microsoft.EntityFrameworkCore;

namespace JobSearch.Services
{
    public class ChatService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        // Event: wywoływany, gdy ktoś wyśle wiadomość. 
        // Parametry: senderId, receiverId
        public event Action<string, string>? OnMessageSent;

        public ChatService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task SendMessageAsync(string senderId, string receiverId, string message)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var msg = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            context.ChatMessages.Add(msg);
            await context.SaveChangesAsync();

            // Powiadom subskrybentów (czyli otwarte okna czatu), że jest nowa wiadomość
            OnMessageSent?.Invoke(senderId, receiverId);
        }

        public async Task<List<ChatMessage>> GetConversationAsync(string userId1, string userId2)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // Pobierz wiadomości między tymi dwoma użytkownikami (w obie strony)
            return await context.ChatMessages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        // Pobiera listę osób, z którymi użytkownik ma "kontakt" (przez aplikacje na oferty)
        public async Task<List<ApplicationUser>> GetChatPartnersAsync(string currentUserId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // 1. Ludzie, którzy aplikowali na moje oferty
            var applicants = await context.JobApplications
                .Where(a => a.JobOffer.CreatedByUserId == currentUserId)
                .Select(a => a.Applicant)
                .Distinct()
                .ToListAsync();

            // 2. Pracodawcy, na których oferty ja aplikowałem
            var employers = await context.JobApplications
                .Where(a => a.ApplicantId == currentUserId)
                .Select(a => a.JobOffer.CreatedByUser)
                .Distinct()
                .ToListAsync();

            // Połącz listy i usuń duplikaty oraz nuli
            var result = applicants.Concat(employers)
                .Where(u => u != null && u.Id != currentUserId)
                .DistinctBy(u => u!.Id)
                .ToList();

            return result!;
        }
    }
}