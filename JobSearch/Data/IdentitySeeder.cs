using Microsoft.AspNetCore.Identity;

namespace JobSearch.Data
{
	public static class IdentitySeeder
	{
		public static async Task SeedAsync(IServiceProvider sp)
		{
			using var scope = sp.CreateScope();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			if (!await roleManager.RoleExistsAsync("Admin"))
				await roleManager.CreateAsync(new IdentityRole("Admin"));

			if (!await roleManager.RoleExistsAsync("Moderator"))
				await roleManager.CreateAsync(new IdentityRole("Moderator"));

			if (!await roleManager.RoleExistsAsync("User"))
				await roleManager.CreateAsync(new IdentityRole("User"));

			var adminEmail = "nowy@janek.pl";
			var admin = await userManager.FindByEmailAsync(adminEmail);
			if (admin is not null)
			{
				await userManager.AddToRoleAsync(admin, "Admin");

			}

			var moderatorEmail = "moderator@janek.pl";
			var moderator = await userManager.FindByEmailAsync(moderatorEmail);
			if (moderator is not null)
			{
				await userManager.AddToRoleAsync(moderator, "Moderator");
			}

		}
	}
}
