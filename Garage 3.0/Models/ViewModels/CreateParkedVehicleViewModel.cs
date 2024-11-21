namespace Garage_3._0.Models.ViewModels
{
    public class CreateParkedVehicleViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }

        //Foreign Keys
        public VehicleType VehicleType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<VehicleType> VehicleTypes { get; set; }

       // public ParkingSpot ParkingSpot { get; set; }
    }
}
