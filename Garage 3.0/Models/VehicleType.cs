﻿namespace Garage_3._0.Models
{
    public class VehicleType
    {
        public string Name { get; set; }

        public ICollection<ParkedVehicle> Vehicles { get; set; }
    }
}
