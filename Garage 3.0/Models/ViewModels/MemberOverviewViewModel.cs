namespace Garage_3._0.Models.ViewModels
{
    public class MemberOverviewViewModel
    {
        public int Id { get; set; } 
        public string MemberId { get; set; }
        public string FullName { get; set; }
        public int NumberOfRegisteredVehicles { get; set; }
        public decimal TotalParkingCost { get; set; }
        public ICollection<VehicleDetailsViewModel> RegisteredVehicles { get; set; }
        public MemberOverviewViewModel()
        {
            RegisteredVehicles = new List<VehicleDetailsViewModel>();
        }
    }
}
