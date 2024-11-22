﻿namespace Garage_3._0.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }

        //Foreign Keys
        public VehicleType VehicleType { get; set; }
        public ApplicationUser ApplicationUser {  get; set; }
        
        public ParkingSpot ParkingSpot { get; set; }

        //Navigational key

    }
}
