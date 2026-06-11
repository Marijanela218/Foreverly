using Bogus;
using Foreverly.Models;

namespace Foreverly.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.PartnerCategories.Any() || context.Weddings.Any())
            return;

        var faker = new Faker("hr");

        // ─── 1. KATEGORIJE ─────────────────────────────
        var categories = new List<PartnerCategory>
        {
            new() { Name = "Bend/DJ" },
            new() { Name = "Cvjećara" },
            new() { Name = "Restoran" },
            new() { Name = "Slastičarnica" },
        };

        context.PartnerCategories.AddRange(categories);
        await context.SaveChangesAsync();

        // ─── 2. PARTNERI ───────────────────────────────
        var partners = new List<Partner>();

        for (int i = 0; i < 10; i++)
        {
            partners.Add(new Partner
            {
                Name = faker.Company.CompanyName(),
                CategoryId = faker.PickRandom(categories).Id,
                Phone = faker.Phone.PhoneNumber(),
                Email = faker.Internet.Email(),
                Address = faker.Address.FullAddress(),
                ContactPerson = faker.Name.FullName(),
                DefaultCommissionPercent = faker.Random.Decimal(5, 15)
            });
        }

        context.Partners.AddRange(partners);
        await context.SaveChangesAsync();

        // ─── 3. BENDOVI ───────────────────────────────
        var bandPartners = partners.Where(p => p.CategoryId == categories[0].Id).Take(3).ToList();

        var bands = bandPartners.Select(p => new Band
        {
            PartnerId = p.Id,
            Description = "Profesionalni bend za svadbe"
        }).ToList();

        context.Bands.AddRange(bands);
        await context.SaveChangesAsync();

        // ─── 4. RESTORANI ─────────────────────────────
        var restaurantPartners = partners.Where(p => p.CategoryId == categories[2].Id).Take(3).ToList();

        var restaurants = restaurantPartners.Select(p => new Restaurant
        {
            PartnerId = p.Id,
            HasWeddingHall = true,
            OffersCatering = true
        }).ToList();

        context.Restaurants.AddRange(restaurants);
        await context.SaveChangesAsync();

        // ─── 5. SALE ──────────────────────────────────
        var halls = new List<Hall>();

        foreach (var r in restaurants)
        {
            halls.Add(new Hall
            {
                RestaurantId = r.PartnerId,
                Name = "Velika sala",
                Capacity = faker.Random.Int(100, 300),
                Address = faker.Address.StreetAddress(),
                BasePrice = faker.Random.Decimal(2000, 5000)
            });
        }

        context.Halls.AddRange(halls);
        await context.SaveChangesAsync();

        // ─── 6. MENI ──────────────────────────────────
        var menus = new List<Menu>();

        foreach (var r in restaurants)
        {
            menus.Add(new Menu
            {
                RestaurantId = r.PartnerId,
                Name = "Svadbeni meni",
                PricePerPerson = faker.Random.Decimal(30, 80)
            });
        }

        context.Menus.AddRange(menus);
        await context.SaveChangesAsync();

        // ─── 7. VJENČANJA ─────────────────────────────
        var weddings = new List<Wedding>();

        for (int i = 0; i < 5; i++)
        {
            weddings.Add(new Wedding
            {
                SuggestedDate = DateOnly.FromDateTime(faker.Date.Future()),
                ConfirmedDate = DateOnly.FromDateTime(faker.Date.Future()),
                Status = "Planned",
                ClientName = faker.Name.FullName(),
                ClientEmail = faker.Internet.Email(),
                ClientPhone = faker.Phone.PhoneNumber(),
                ClientAddress = faker.Address.FullAddress()
            });
        }

        context.Weddings.AddRange(weddings);
        await context.SaveChangesAsync();

        // ─── 8. GOSTI ────────────────────────────────
        // 🔥 FIX: uvijek generiši goste ako ih nema
        if (!context.Guests.Any())
        {
            var guests = new List<Guest>();

            foreach (var w in weddings)
            {
                for (int i = 0; i < 50; i++)
                {
                    guests.Add(new Guest
                    {
                        WeddingId = w.Id,
                        FullName = faker.Name.FullName(),
                        Side = faker.PickRandom(new[] { "Bride", "Groom" }),
                        Phone = faker.Phone.PhoneNumber()
                    });
                }
            }

            context.Guests.AddRange(guests);
            await context.SaveChangesAsync();
        }

        // ─── 9. STOLOVI ──────────────────────────────
        var tables = new List<WeddingTable>();

        foreach (var w in weddings)
        {
            for (int i = 1; i <= 12; i++)
            {
                tables.Add(new WeddingTable
                {
                    WeddingId = w.Id,
                    TableName = $"Table {i}",
                    Capacity = 10
                });
            }
        }

        context.WeddingTables.AddRange(tables);
        await context.SaveChangesAsync();
    }
}