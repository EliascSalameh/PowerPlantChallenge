using System.Text.Json.Serialization;

namespace REST.Engine.Entities
{
    public class Fuels
    {
        [JsonPropertyName("gas(euro/MWh)")] public double GasPrice { get; set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public double KerosinePrice { get; set; }

        [JsonPropertyName("co2(euro/ton)")] public double Co2Consumption { get; set; }

        [JsonPropertyName("wind(%)")] public double WindPercentage { get; set; }
    }
}