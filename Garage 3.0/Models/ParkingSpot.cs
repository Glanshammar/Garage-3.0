namespace Garage_3._0.Models
{
    public class ParkingSpot
    {
        //Primary key
       public int Id { get; set; }
        public int ParkingSpotId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Occupied { get; set; }
        
    }
}
