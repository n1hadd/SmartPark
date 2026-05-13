using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartPark.Models;

namespace SmartPark.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var ctx = scope.ServiceProvider.GetRequiredService<SmartParkContext>();

        // Vloge
        var roles = new[] { "Administrator", "Manager", "Staff" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Demo uporabnik
        var adminEmail = "admin@smartpark.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Smart",
                LastName = "Admin",
                EmailConfirmed = true,
                RegistrskaStevilka = "GO-1234"
            };
            await userManager.CreateAsync(admin, "Admin!234");
            await userManager.AddToRoleAsync(admin, "Administrator");
        }

    }

    public static async Task SeedOverpassParking(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SmartParkContext>();

        // enkratni import
        if (await ctx.Parkirisca.AnyAsync())
            return;

        try
        {
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(120) // Overpass je počasen
            };

            var api = new OverpassApiHelper(httpClient);
            var parkirisca = await api.GetParkiriscaLjubljanaAsync();

            if (parkirisca.Count == 0)
            {
                Console.WriteLine("Overpass import: 0 results.");
                return;
            }

            ctx.Parkirisca.AddRange(parkirisca);
            await ctx.SaveChangesAsync();

            var parks = await ctx.Parkirisca
                .Include(p => p.ParkirnaMesta)
                .ToListAsync();

            foreach (var p in parks)
            {
                if (p.ParkirnaMesta != null && p.ParkirnaMesta.Count > 0)
                    continue;

                var count = p.SteviloMest > 0 ? p.SteviloMest : 30;

                // varnostna omejitev, da ne ustvarjaš tisoče mest po pomoti
                if (count > 200) count = 200;

                for (int i = 1; i <= count; i++)
                {
                    ctx.ParkirnaMesta.Add(new ParkirnoMesto
                    {
                        ParkirisceId = p.Id,
                        Stevilka = i,
                        Zasedeno = false,
                        Tip = TipMesta.Navadno
                    });
                }
            }

            await ctx.SaveChangesAsync();

            Console.WriteLine($"Overpass import OK: imported {parkirisca.Count} parkirisca.");
        }
        catch (Exception ex)
        {
            // pomembno: app naj vseeno normalno štarta
            Console.WriteLine("Overpass import FAILED (app will continue): " + ex.Message);
        }
    }
}