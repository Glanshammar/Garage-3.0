namespace Garage_3._0.Models.ViewModels
{
    public class ParkingStatisticsViewModel
    {
        public int TotalParkingSpots { get; set; }
        public int OccupiedParkingSpots { get; set; }
        public int AvailableParkingSpots { get; set; }
        public double OccupiedPercentage { get; set; }
        public double AvailablePercentage { get; set; }
        public List<SizeStatistic> SizeStatistics { get; set; }
    }

    public class SizeStatistic
    {
        public string Size { get; set; }
        public int TotalSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int AvailableSpots { get; set; }
    }
}
