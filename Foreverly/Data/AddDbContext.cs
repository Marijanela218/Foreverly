using Microsoft.EntityFrameworkCore;
using Foreverly.Models;

namespace Foreverly.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt)
            : base(opt) { }

        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateItem> TemplateItems { get; set; }

        public DbSet<PartnerCategory> PartnerCategories { get; set; }
        public DbSet<Partner> Partners { get; set; }

        public DbSet<Band> Bands { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongGroup> SongGroups { get; set; }
        public DbSet<PlaylistItem> PlaylistItems { get; set; }
        public DbSet<BandPrice> BandPrices { get; set; }

        public DbSet<FloralArrangement> FloralArrangements { get; set; }
        public DbSet<PastryItem> PastryItems { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<WeddingService> WeddingServices { get; set; }
        public DbSet<WeddingServiceItem> WeddingServiceItems { get; set; }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<WeddingTable> WeddingTables { get; set; }
        public DbSet<SeatingAssignment> SeatingAssignments { get; set; }

        public DbSet<PartnerBooking> PartnerBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:1 Partner -> Band
            modelBuilder.Entity<Band>()
                .HasKey(b => b.PartnerId);

            modelBuilder.Entity<Band>()
                .HasOne(b => b.Partner)
                .WithOne(p => p.Band)
                .HasForeignKey<Band>(b => b.PartnerId);

            // 1:1 Partner -> Restaurant
            modelBuilder.Entity<Restaurant>()
                .HasKey(r => r.PartnerId);

            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Partner)
                .WithOne(p => p.Restaurant)
                .HasForeignKey<Restaurant>(r => r.PartnerId);

            // SeatingAssignment: Guest 1:1 Assignment
            modelBuilder.Entity<SeatingAssignment>()
                .HasOne(sa => sa.Guest)
                .WithOne(g => g.SeatingAssignment)
                .HasForeignKey<SeatingAssignment>(sa => sa.GuestId);

            // WeddingTable ime tabele u bazi
            modelBuilder.Entity<WeddingTable>()
                .ToTable("Tables");

            // Decimal preciznost
            modelBuilder.Entity<Partner>()
                .Property(p => p.DefaultCommissionPercent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BandPrice>()
                .Property(bp => bp.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FloralArrangement>()
                .Property(f => f.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PastryItem>()
                .Property(p => p.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Hall>()
                .Property(h => h.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Menu>()
                .Property(m => m.PricePerPerson)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WeddingService>()
                .Property(ws => ws.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WeddingService>()
                .Property(ws => ws.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WeddingService>()
                .Property(ws => ws.CommissionPercent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WeddingService>()
                .Property(ws => ws.CommissionAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WeddingServiceItem>()
                .Property(wsi => wsi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WeddingServiceItem>()
                .Property(wsi => wsi.TotalPrice)
                .HasPrecision(18, 2);
        }
    }
}