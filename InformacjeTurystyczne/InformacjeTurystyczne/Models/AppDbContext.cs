using InformacjeTurystyczne.Models.Tabels;
using Microsoft.EntityFrameworkCore; // DbContext

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace InformacjeTurystyczne.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Party> Partys { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PermissionParty> PermissionPartys { get; set; }
        public DbSet<PermissionRegion> PermissionRegions { get; set; }
        public DbSet<PermissionShelter> PermissionShelters { get; set; }
        public DbSet<PermissionTrail> PermissionTrails { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<RegionLocation> RegionLocations { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public override DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Party>().ToTable("Party");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<PermissionParty>().ToTable("PermissionParty");
            modelBuilder.Entity<PermissionRegion>().ToTable("PermissionRegion");
            modelBuilder.Entity<PermissionShelter>().ToTable("PermissionShelter");
            modelBuilder.Entity<PermissionTrail>().ToTable("PermissionTrail");
            modelBuilder.Entity<Region>().ToTable("Region");
            modelBuilder.Entity<Shelter>().ToTable("Shelter");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<RegionLocation>().ToTable("RegionLocation");
            modelBuilder.Entity<Trail>().ToTable("Trail");
            modelBuilder.Entity<Attraction>().ToTable("Attraction");
            modelBuilder.Entity<AppUser>().ToTable("AspNetUsers");

            modelBuilder.Entity<Party>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.Party)
                .HasForeignKey(s => s.IdRegion);

            modelBuilder.Entity<Attraction>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.Attraction)
                .HasForeignKey(bc => bc.IdRegion);

            modelBuilder.Entity<Category>()
                .HasMany<Message>(bc => bc.Messages)
                .WithOne(c => c.Category)
                .HasForeignKey(s => s.IdCategory);
                
            modelBuilder.Entity<Message>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.Message)
                .HasForeignKey(s => s.IdRegion)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Message>()
                .HasOne<Category>(bc => bc.Category)
                .WithMany(c => c.Messages)
                .HasForeignKey(s => s.IdCategory)
                .OnDelete(DeleteBehavior.SetNull);

            /*
            modelBuilder.Entity<PermissionParty>()
                .HasOne<Party>(bc => bc.Party)
                .WithMany(c => c.PermissionParty)
                .HasForeignKey(s => s.IdParty);

            modelBuilder.Entity<PermissionParty>()
                .HasOne<AppUser>(bc => bc.User)
                .WithMany(c => c.PermissionPartys)
                .HasForeignKey(s => s.IdUser);

            modelBuilder.Entity<PermissionRegion>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.PermissionRegion)
                .HasForeignKey(c => c.IdRegion);

            modelBuilder.Entity<PermissionRegion>()
                .HasOne<AppUser>(bc => bc.User)
                .WithMany(c => c.PermissionRegions)
                .HasForeignKey(s => s.IdUser);

            modelBuilder.Entity<PermissionShelter>()
                .HasOne<Shelter>(bc => bc.Shelter)
                .WithMany(c => c.PermissionShelters)
                .HasForeignKey(s => s.IdShelter);

            modelBuilder.Entity<PermissionShelter>()
                .HasOne<AppUser>(bc => bc.User)
                .WithMany(c => c.PermissionShelters)
                .HasForeignKey(s => s.IdUser);

            
            modelBuilder.Entity<PermissionTrail>()
                .HasOne<Trail>(bc => bc.Trail)
                .WithMany(c => c.PermissionTrail)
                .HasForeignKey(s => s.IdTrail);

            modelBuilder.Entity<PermissionTrail>()
                .HasOne<AppUser>(bc => bc.User)
                .WithMany(c => c.PermissionTrails)
                .HasForeignKey(s => s.IdUser);
                */

            modelBuilder.Entity<PermissionParty>()
                .HasKey(c => new { c.IdParty, c.IdUser });

            modelBuilder.Entity<PermissionRegion>()
                .HasKey(c => new { c.IdRegion, c.IdUser });

            modelBuilder.Entity<PermissionShelter>()
                .HasKey(c => new { c.IdShelter, c.IdUser });

            modelBuilder.Entity<PermissionTrail>()
                .HasKey(c => new { c.IdTrail, c.IdUser });

            modelBuilder.Entity<RegionLocation>()
                .HasKey(c => new { c.IdRegion, c.IdTrail });

            //REGION??? chyba pozostałe funkcje WithMany() to załatwią (?)

            modelBuilder.Entity<RegionLocation>()
                .HasOne<Trail>(bc => bc.Trail)
                .WithMany(c => c.RegionLocation)
                .HasForeignKey(s => s.IdTrail);

            modelBuilder.Entity<RegionLocation>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.RegionLocation)
                .HasForeignKey(s => s.IdRegion);

            modelBuilder.Entity<Shelter>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.Shelter)
                .HasForeignKey(s => s.IdRegion);

            modelBuilder.Entity<Subscription>()
                .HasOne<Region>(bc => bc.Region)
                .WithMany(c => c.Subscription)
                .HasForeignKey(s => s.IdRegion);

            modelBuilder.Entity<Subscription>()
                .HasOne<AppUser>(bc => bc.User)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.IdUser);

            // TRIAL ??? chyba też załatwiony przez funkcje WithMany w pozostałych wywołaniach

            modelBuilder.Entity<AppUser>()
                .HasMany(i => i.Subscriptions)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.IdUser);

            modelBuilder.Entity<AppUser>()
                .HasMany(i => i.PermissionPartys)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.IdUser);

            modelBuilder.Entity<AppUser>()
                .HasMany(i => i.PermissionRegions)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.IdUser);

            modelBuilder.Entity<AppUser>()
                .HasMany(i => i.PermissionTrails)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.IdUser);

            modelBuilder.Entity<AppUser>()
                .HasMany(i => i.PermissionShelters)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.IdUser);

            modelBuilder.Entity<Region>()
                .HasMany(i => i.RegionLocation)
                .WithOne(i => i.Region)
                .HasForeignKey(i => i.IdRegion);


        }
    }
}
