using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models.ViewModels
{
    public class ParkedVehicleCreateViewModel
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

        [Required(ErrorMessage = "Vehicle Type is required.")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Parking Spot is required.")]
        public int ParkingSpotId { get; set; }

        // Optionally? 
        public string ApplicationUserId { get; set; }
    }
}
