namespace Garage_3._0.Models
{
    public class ParkingSpotParkedVehicle
    {
        public int ParkingSpotId { get; set; }
        public string RegistrationNumber { get; set; }
        public ParkingSpot ParkingSpot { get; set; }
        public ParkedVehicle ParkedVehicle { get; set; }
    }
}
