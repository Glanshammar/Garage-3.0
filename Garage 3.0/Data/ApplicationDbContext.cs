using Garage_3._0.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage_3._0.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ParkedVehicle> ParkedVehicles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VehicleType>()
                .HasKey(t => new { t.Name });

           builder.Entity<ApplicationUser>()
          .HasIndex(u => u.PersonalNumber)
          .IsUnique();


            // Testdata för ParkingSpots
            builder.Entity<ParkingSpot>().HasData(
                new ParkingSpot { Id = 1, SpotNumber = "A1", Size = "Small", Location = "North", IsOccupied = false },
                new ParkingSpot { Id = 2, SpotNumber = "B2", Size = "Medium", Location = "South", IsOccupied = true },
                new ParkingSpot { Id = 3, SpotNumber = "C3", Size = "Large", Location = "East", IsOccupied = false }
            );

            // Testdata för VehicleType
            builder.Entity<VehicleType>().HasData(
                new VehicleType { Name = "Car" },
                new VehicleType { Name = "Motorcycle" },
                new VehicleType { Name = "Bus" }
            );



        }

      
    }
}
