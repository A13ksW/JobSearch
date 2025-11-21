using JobSearch.Data;
using Microsoft.EntityFrameworkCore;

namespace JobSearch.Services
{
    public class ExpiredOfferService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly ILogger<ExpiredOfferService> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ExpiredOfferService(ILogger<ExpiredOfferService> logger, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serwis wygaszania ofert został uruchomiony.");

            // POPRAWKA: Zmieniono interwał z 24 godzin na 1 godzinę.
            // Uruchom zadanie natychmiast, a następnie powtarzaj co godzinę.
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            _logger.LogInformation("Serwis wygaszania ofert rozpoczyna pracę...");

            await using var context = await _contextFactory.CreateDbContextAsync();

            var now = DateTime.UtcNow;

            // Znajdź wszystkie oferty, które są "Opublikowane", ale ich data ważności minęła.
            var expiredOffers = await context.JobOffer
                .Where(o => o.Status == OfferStatus.Opublikowana &&
                            o.ApplicationDeadline.HasValue &&
                            o.ApplicationDeadline.Value < now)
                .ToListAsync();

            if (expiredOffers.Any())
            {
                foreach (var offer in expiredOffers)
                {
                    offer.Status = OfferStatus.Wygasła;
                }

                await context.SaveChangesAsync();
                _logger.LogInformation($"Zaktualizowano {expiredOffers.Count} ofert jako 'Wygasłe'.");
            }
            else
            {
                _logger.LogInformation("Nie znaleziono ofert do wygaszenia.");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serwis wygaszania ofert został zatrzymany.");

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}