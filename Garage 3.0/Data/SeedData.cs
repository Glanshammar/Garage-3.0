using Garage_3._0.Models;
using Microsoft.AspNetCore.Identity;

namespace Garage_3._0.Data
{
    public class SeedData
    {

        private static ApplicationDbContext context = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<ApplicationUser> userManager = default!;

        public static async Task Init(ApplicationDbContext _context, IServiceProvider services)
        {
            context = _context;
            if (context.Roles.Any()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var roleNames = new[] { "User", "Admin" };
            var adminEmail = "admin@admin.com";
            var userEmail = "user@user.com";
            var minorUserEmail = "minoruser@user.com";

            await AddRolesAsync(roleNames);

            var admin = await AddAccountAsync(adminEmail, "AdminName", "AdminLName", "Admin2024-", "199701037990");
            var user = await AddAccountAsync(userEmail, "UserName", "UserLName", "User2024-", "199504113240");
            var minorUser = await AddAccountAsync(minorUserEmail, "MinorUserName", "MinorUserLName", "User2024-", "201005111234");

            await AddUserToRoleAsync(admin, "Admin");
            await AddUserToRoleAsync(user, "User");
            await AddUserToRoleAsync(minorUser, "User");

            // Seed parking spots
            await AddParkingSpotsAsync();
        }


        private static async Task AddParkingSpotsAsync()
        {
            if (context.ParkingSpots.Any()) return;

            var parkingSpots = new List<ParkingSpot>
        {
            // Ground Floor
            new ParkingSpot { SpotNumber = "A1", Size = "Small", Location = "Ground Floor - North", IsOccupied = false, ParkingCost = 20.00m },
            new ParkingSpot { SpotNumber = "A2", Size = "Small", Location = "Ground Floor - West", IsOccupied = false, ParkingCost = 20.00m },
            new ParkingSpot { SpotNumber = "A3", Size = "Medium", Location = "Ground Floor - East", IsOccupied = false, ParkingCost = 30.00m },
            new ParkingSpot { SpotNumber = "A4", Size = "Medium", Location = "Ground Floor - South", IsOccupied = false, ParkingCost = 30.00m },


            // First Floor 
            new ParkingSpot { SpotNumber = "1A1", Size = "Small", Location = "First Floor - North", IsOccupied = false, ParkingCost = 15.00m },
            new ParkingSpot { SpotNumber = "1A2", Size = "Small", Location = "First Floor - West", IsOccupied = false, ParkingCost = 15.00m },
            new ParkingSpot { SpotNumber = "1A3", Size = "Medium", Location = "First Floor - East", IsOccupied = false, ParkingCost = 25.00m },
            new ParkingSpot { SpotNumber = "1A4", Size = "Large", Location = "First Floor - South", IsOccupied = false, ParkingCost = 35.00m },

            // Second Floor 
            new ParkingSpot { SpotNumber = "2A1", Size = "Small", Location = "Second Floor  - North", IsOccupied = false, ParkingCost = 15.00m },
            new ParkingSpot { SpotNumber = "2A2", Size = "Medium", Location = "Second Floor - West", IsOccupied = false, ParkingCost = 25.00m },
            new ParkingSpot { SpotNumber = "2A3", Size = "Medium", Location = "Second Floor - East", IsOccupied = false, ParkingCost = 25.00m },
            new ParkingSpot { SpotNumber = "2A4", Size = "Large", Location = "Second Floor - South", IsOccupied = false, ParkingCost = 35.00m },
           
        };

            await context.ParkingSpots.AddRangeAsync(parkingSpots);
            await context.SaveChangesAsync();
        }

        private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<ApplicationUser> AddAccountAsync(string accountEmail, string fName, string lName, string pw, string personalNumber)
        {
            var found = await userManager.FindByEmailAsync(accountEmail);

            if (found != null) return null!;

            var user = new ApplicationUser
            {
                UserName = accountEmail,
                Email = accountEmail,
                FirstName = fName,
                LastName = lName,
                PersonalNumber = personalNumber,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, pw);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return user;
        }
    }


}
