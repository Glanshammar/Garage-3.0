namespace Garage_3._0.Models.ViewModels
{
    public class ParkingSpotViewModel
    {
        public int Id { get; set; }
        public string SpotNumber { get; set; }
        public string Size { get; set; }
        public string Location { get; set; }
        public bool IsOccupied { get; set; }
       // public string AssignedVehicleRegistration { get; set; } 
        public decimal ParkingCost { get; set; }
        //public string OwnerName { get; set; }
    }
}
