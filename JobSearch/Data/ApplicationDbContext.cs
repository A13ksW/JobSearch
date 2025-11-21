using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobSearch.Services;

namespace JobSearch.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        // Główne tabele
        public DbSet<JobOffer> JobOffer { get; set; } = default!;
        public DbSet<JobApplication> JobApplications { get; set; } = default!;

        // Systemowe
        public DbSet<AuditLog> AuditLogs { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; } = default!;

        // --- NOWE: Tabela Czatu ---
        public DbSet<ChatMessage> ChatMessages { get; set; } = default!;

        // CV Kandydata
        public DbSet<UserProfileCV> UserProfileCVs { get; set; } = default!;
        public DbSet<CVSkill> CVSkills { get; set; } = default!;
        public DbSet<CVLanguage> CVLanguages { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // === KONFIGURACJA RELACJI UŻYTKOWNIKA ===

            // 1. Użytkownik -> Stworzone Oferty
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.CreatedJobOffers)
                .WithOne(o => o.CreatedByUser)
                .HasForeignKey(o => o.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Użytkownik -> Złożone Aplikacje
            // RESTRICT, aby uniknąć błędu "multiple cascade paths" w SQL Server
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Applications)
                .WithOne(a => a.Applicant)
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Użytkownik -> Powiadomienia
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. Użytkownik -> Profil CV
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.UserProfileCV)
                .WithOne(cv => cv.User)
                .HasForeignKey<UserProfileCV>(cv => cv.UserId);


            // === INNE RELACJE ===

            // Oferta -> Aplikacje
            builder.Entity<JobOffer>()
                .HasMany(o => o.Applications)
                .WithOne(a => a.JobOffer)
                .HasForeignKey(a => a.JobOfferId)
                .OnDelete(DeleteBehavior.Cascade);

            // Profil CV -> Języki
            builder.Entity<UserProfileCV>()
                .HasMany(cv => cv.Languages)
                .WithOne(l => l.UserProfileCV)
                .HasForeignKey(l => l.UserProfileCVId)
                .OnDelete(DeleteBehavior.Cascade);

            // Profil CV -> Umiejętności
            builder.Entity<UserProfileCV>()
                .HasMany(cv => cv.Skills)
                .WithOne(s => s.UserProfileCV)
                .HasForeignKey(s => s.UserProfileCVId)
                .OnDelete(DeleteBehavior.Cascade);

            // === RELACJE CZATU ===
            // Używamy Restrict dla nadawcy, aby uniknąć pętli usuwania (multiple cascade paths)
            builder.Entity<ChatMessage>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}