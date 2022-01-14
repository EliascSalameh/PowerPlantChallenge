using System.Text.Json.Serialization;
using REST.Engine.Enum;

namespace REST.Engine.Entities
{
    public class PowerPlants
    {
        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PowerPlantTypeEnum Type { get; set; }

        public double Efficiency { get; set; }
        public double PMax { get; set; }
        public double PMin { get; set; }
    }
}