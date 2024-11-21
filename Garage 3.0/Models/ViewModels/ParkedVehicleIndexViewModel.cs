namespace Garage_3._0.Models.ViewModels
{
    public class ParkedVehicleIndexViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public VehicleType VehicleType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ParkingSpot ParkingSpot { get; set; }
    }
}
