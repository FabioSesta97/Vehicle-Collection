using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Demo2
{
    public class Items
    {
        [JsonPropertyName("Cars")]
        public List<Car> Cars { get; set; }
        [JsonPropertyName("Trucks")]
        public List<Truck> Trucks { get; set; }
        [JsonPropertyName("Bikes")]
        public List<Bike> Bikes { get; set; }
    }
}