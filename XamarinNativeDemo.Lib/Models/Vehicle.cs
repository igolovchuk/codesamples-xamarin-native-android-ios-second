using System;
namespace XamarinNativeDemo.Models
{
    public class Vehicle
    {
        public string Id { get; set; }

        public string ModelId { get; set; }

        // Korobka peredach
        public string GearId { get; set; }

        public string Year { get; set; }

        public string EngineVolume { get; set; }

        public string FuelId { get; set; }

        public string IsNotLegalized { get; set; }

        //------statistic data---------------//
        public string DeviceName { get; set; }

        public string DeviceId { get; set; }

        // For Pandora service
        public string Identifier { get; set; }
    }
}
