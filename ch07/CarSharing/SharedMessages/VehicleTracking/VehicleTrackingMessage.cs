using SharedMessages.BasicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMessages.VehicleTracking
{
    public class VehicleTrackingMessage : TimedMessage
    {
        public Guid VehicleId { get; set; }
        public GeoLocalizationMessage? Location { get; set; }
        public double Speed { get; set; }
        public double CarStatus { get; set; }
        public double BatteryLevel { get; set; }
        public double FuelLevel { get; set; }
        public double TirePressure { get; set; }    
    }
    
}
