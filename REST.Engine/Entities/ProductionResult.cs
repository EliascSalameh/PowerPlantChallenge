using System.Text.Json.Serialization;

namespace REST.Engine.Entities
{
    public class ProductionResult
    {
        [JsonPropertyName("name")] public string PowerPlantName { get; set; }

        [JsonPropertyName("p")] public double Production { get; set; }
    }
}