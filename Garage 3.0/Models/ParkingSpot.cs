namespace Garage_3._0.Models
{
    public class ParkingSpot
    {
        //Primary key
        public int Id { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        ParkedVehicle? ParkedVehicle { get; set; }
        
    }
}
