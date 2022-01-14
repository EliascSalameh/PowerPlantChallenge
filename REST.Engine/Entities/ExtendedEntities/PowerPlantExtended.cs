namespace REST.Engine.Entities.ExtendedEntities
{
    public class PowerPlantExtended : PowerPlants
    {
        public double ActualFuelCostPerMWh { get; set; }
        public double ActualPMax { get; set; }
    }
}