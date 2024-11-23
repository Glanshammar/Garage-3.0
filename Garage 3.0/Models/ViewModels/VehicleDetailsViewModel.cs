namespace Garage_3._0.Models.ViewModels
{
    public class VehicleDetailsViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string VehicleType { get; set; }
        public decimal CurrentParkingCost { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string ParkingSpotNumber { get; set; }  // Assigned parking spot number
        public TimeSpan ParkingDuration { get; set; }  // Duration the vehicle has been parked
        public decimal TotalParkingCost { get; set; }
    }
}
