using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XamarinNativeDemo.Models
{
    public class AverageValueItem
    {
        public int Total { get; set; }

        public decimal ArithmeticMean { get; set; }

        public decimal InterQuartileMean { get; set; }

        public Dictionary<string, string> Percentiles { get; set; }
    }
}
