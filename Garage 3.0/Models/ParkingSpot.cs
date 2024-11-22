using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        [Required]
        public string SpotNumber { get; set; } // Unik identifierare för plats

        [Required]
        public string Size { get; set; } // Small, Medium, Large

        [Required]
        public string Location { get; set; } 
        public bool IsOccupied { get; set; }
        public decimal ParkingCost { get; set; }

        // Relationship to ParkedVehicle
         public ParkedVehicle ParkedVehicle { get; set; }
    }
}
