using Garage_3._0.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Registration number is required.")]
        [RegularExpression(@"^[A-HJ-PR-UW-Z]{3}\d{3}$", ErrorMessage = "Registration number must be in the format ABC123.")]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [ValidVehicleBrand(ErrorMessage = "Please enter a valid vehicle brand.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        [ValidVehicleColor(ErrorMessage = "Please enter a valid vehicle color.")]
        public string Color { get; set; }

        // Foreign keys
        [Required(ErrorMessage = "Vehicle type is required.")]
        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int? ParkingSpotId { get; set; } // Note: Nullable if a vehicle may not always be parked

        // Navigation property to ParkingSpot
        [Required(ErrorMessage = "Parking spot is required.")]
        [Display(Name = "Parking Spot")]
        public ParkingSpot ParkingSpot { get; set; }
    }
}