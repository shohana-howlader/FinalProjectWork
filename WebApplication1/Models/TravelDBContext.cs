using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class TravelDBContext : DbContext
    {
        public TravelDBContext()
        {
            
        }
        public TravelDBContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<PackageFacility> PackageFacilities { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This ensures Identity configuration is included

            // Define relationships for Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Package)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PackageId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);
        }

    }
}
