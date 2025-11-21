using JobSearch.Components;
using JobSearch.Data;
using JobSearch.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Razor Components (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Auth / Identity helper services
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"))
    .AddPolicy("RequireModerator", policy => policy.RequireRole("Moderator"));

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("CanSearchJobs", policy => policy.RequireClaim("account_type", "Individual"));
});

// --- BAZA DANYCH ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1) Fabryka DbContext (używana w serwisach domenowych)
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// 2) Zwykły DbContext (używany przez Identity / Razor Pages)
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString);
    options.UseApplicationServiceProvider(sp);
});

// --- SERWISY APLIKACJI ---
builder.Services.AddScoped<IJobOfferService, JobOfferService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddHostedService<ExpiredOfferService>();

builder.Services.AddScoped<INipValidationService, NipValidationService>();
builder.Services.AddHttpClient<INipValidationService, NipValidationService>(client =>
{
    client.BaseAddress = new Uri("https://wl-api.mf.gov.pl/");
});

// NOWE: lokalny serwis geolokalizacji z pliku Data/polish-locations.json
builder.Services.AddSingleton<IGeoLocationService, LocalGeoLocationService>();

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- IDENTITY ---
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;

    options.SignIn.RequireConfirmedAccount = false; // demo
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Nagłówki bezpieczeństwa
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    ctx.Response.Headers["X-Frame-Options"] = "DENY";
    ctx.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    ctx.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    ctx.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data:; " +
        "font-src 'self' data:; " +
        "connect-src 'self' wss:; " +
        "object-src 'none'; " +
        "frame-ancestors 'none'";
    await next();
});

// seed ról / kont
await IdentitySeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Identity /Account endpoints
app.MapAdditionalIdentityEndpoints();

app.Run();
