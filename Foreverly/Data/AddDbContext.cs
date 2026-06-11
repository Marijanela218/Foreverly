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

            // =====================================================
            // BAND FIX
            // =====================================================
            modelBuilder.Entity<Band>()
                .HasKey(b => b.PartnerId);

            modelBuilder.Entity<Band>()
                .HasOne(b => b.Partner)
                .WithOne(p => p.Band)
                .HasForeignKey<Band>(b => b.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Band>()
                .ToTable("Bands");

            // =====================================================
            // RESTAURANT FIX
            // =====================================================
            modelBuilder.Entity<Restaurant>()
                .HasKey(r => r.PartnerId);

            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Partner)
                .WithOne(p => p.Restaurant)
                .HasForeignKey<Restaurant>(r => r.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Restaurant>()
                .ToTable("Restaurants");

            // =====================================================
            // SEATING FIX (🔥 ISPRAVNO - BEZ CONFLICTA)
            // =====================================================
            modelBuilder.Entity<SeatingAssignment>()
                .HasOne(sa => sa.Guest)
                .WithMany()
                .HasForeignKey(sa => sa.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeatingAssignment>()
                .HasOne(sa => sa.Table)
                .WithMany(t => t.SeatingAssignments)
                .HasForeignKey(sa => sa.TableId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================================================
            // TABLE RENAME
            // =====================================================
            modelBuilder.Entity<WeddingTable>()
                .ToTable("Tables");
        }
    }
}