using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        [Required]
        public string SpotNumber { get; set; } // Unique identifier for the spot

        [Required]
        public string Size { get; set; } // Small, Medium, Large

        [Required]
        public string Location { get; set; } 
        public bool IsOccupied { get; set; }

        // Navigation property for the parked vehicle
        public ParkedVehicle ParkedVehicle { get; set; }
    }
}
