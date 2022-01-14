using System.Collections.Generic;

namespace REST.Engine.Entities
{
    public class Combination
    {
        public double TotalCost { get; set; }

        public double RemainingLoad { get; set; }

        public double TotalPowerProduction { get; set; }

        public int CombinationNumber { get; set; }

        public List<ProductionResult> ProductionResults { get; set; } = new();

        public string Message { get; set; }
    }
}