using Bogus;
using Foreverly.Models;

namespace Foreverly.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.PartnerCategories.Any()) return;

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
            var menu = new Menu
            {
                RestaurantId = r.PartnerId,
                Name = "Svadbeni meni",
                PricePerPerson = faker.Random.Decimal(30, 80)
            };

            menus.Add(menu);
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
        var guests = new List<Guest>();

        foreach (var w in weddings)
        {
            for (int i = 0; i < 20; i++)
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

        // ─── 9. STOLOVI ──────────────────────────────
        var tables = new List<WeddingTable>();

        foreach (var w in weddings)
        {
            for (int i = 1; i <= 5; i++)
            {
                tables.Add(new WeddingTable
                {
                    WeddingId = w.Id,
                    TableName = $"Sto {i}",
                    Capacity = 10
                });
            }
        }

        context.WeddingTables.AddRange(tables);
        await context.SaveChangesAsync();

        // ─── 10. USLUGE ──────────────────────────────
        var services = new List<WeddingService>();

        foreach (var w in weddings)
        {
            var randomPartner = faker.PickRandom(partners);

            services.Add(new WeddingService
            {
                WeddingId = w.Id,
                PartnerId = randomPartner.Id,
                ServiceType = "General",
                Quantity = 1,
                UnitPrice = faker.Random.Decimal(500, 2000),
                TotalPrice = faker.Random.Decimal(500, 2000),
                CommissionPercent = 10,
                CommissionAmount = 100,
                Confirmed = true
            });
        }

        context.WeddingServices.AddRange(services);
        await context.SaveChangesAsync();
        // ─── 11. SONG GROUPS ─────────────────────────────
        var songGroups = new List<SongGroup>
{
    new() { Name = "80s Rock", Description = "Rock klasici" },
    new() { Name = "Pop", Description = "Popularna muzika" },
    new() { Name = "Narodna", Description = "Balkanska muzika" }
};

        context.SongGroups.AddRange(songGroups);
        await context.SaveChangesAsync();


        // ─── 12. SONGS ───────────────────────────────────
        var songs = new List<Song>();

        for (int i = 0; i < 20; i++)
        {
            songs.Add(new Song
            {
                Title = faker.Music.Genre() + " Song",
                Artist = faker.Name.FullName(),
                Genre = faker.Music.Genre()
            });
        }

        context.Songs.AddRange(songs);
        await context.SaveChangesAsync();


        // ─── 13. PLAYLISTS ───────────────────────────────
        var playlists = new List<Playlist>();

        foreach (var band in bands)
        {
            playlists.Add(new Playlist
            {
                BandId = band.PartnerId,
                Name = "Svadbena playlist",
                Description = "Top pjesme za svadbu"
            });
        }

        context.Playlists.AddRange(playlists);
        await context.SaveChangesAsync();


        // ─── 14. PLAYLIST ITEMS ──────────────────────────
        var playlistItems = new List<PlaylistItem>();

        foreach (var pl in playlists)
        {
            for (int i = 0; i < 10; i++)
            {
                playlistItems.Add(new PlaylistItem
                {
                    PlaylistId = pl.Id,
                    SongId = faker.PickRandom(songs).Id
                });
            }
        }

        context.PlaylistItems.AddRange(playlistItems);
        await context.SaveChangesAsync();


        // ─── 15. FLORAL ARRANGEMENTS ─────────────────────
        var floralPartners = partners.Where(p => p.CategoryId == categories[1].Id).ToList();

        var florals = new List<FloralArrangement>();

        foreach (var p in floralPartners)
        {
            florals.Add(new FloralArrangement
            {
                PartnerId = p.Id,
                Name = "Dekoracija stolova",
                Description = "Cvjetni aranžman",
                BasePrice = faker.Random.Decimal(50, 200)
            });
        }

        context.FloralArrangements.AddRange(florals);
        await context.SaveChangesAsync();


        // ─── 16. PASTRY ITEMS ────────────────────────────
        var pastryPartners = partners.Where(p => p.CategoryId == categories[3].Id).ToList();

        var pastries = new List<PastryItem>();

        foreach (var p in pastryPartners)
        {
            pastries.Add(new PastryItem
            {
                PartnerId = p.Id,
                Name = "Svadbena torta",
                Type = "Cake",
                Description = "Velika torta",
                BasePrice = faker.Random.Decimal(100, 500)
            });
        }

        context.PastryItems.AddRange(pastries);
        await context.SaveChangesAsync();
    }
}