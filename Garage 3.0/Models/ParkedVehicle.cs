namespace Garage_3._0.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }

        // Foreign keys
        public VehicleType VehicleType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int? ParkingSpotId { get; set; } // Note: Nullable if a vehicle may not always be parked
        
        // Navigation property to ParkingSpot
        public ParkingSpot ParkingSpot { get; set; }
    }
}
