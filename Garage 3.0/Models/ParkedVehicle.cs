using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Registration Number is required.")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; }

        // Foreign Keys
        [Required(ErrorMessage = "Vehicle Type is required.")]
        public int VehicleTypeId { get; set; } // Add foreign key for VehicleType
        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public string ApplicationUserId { get; set; } // Foreign key for ApplicationUser
        public ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "Parking Spot is required.")]
        public int ParkingSpotId { get; set; } // Add foreign key for ParkingSpot
        public ParkingSpot ParkingSpot { get; set; }

    }

}
