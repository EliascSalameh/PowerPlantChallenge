using System;
using System.Collections.Generic;
using System.Linq;
using REST.Engine.Entities;
using REST.Engine.Entities.ExtendedEntities;
using REST.Engine.Enum;

namespace REST.Engine
{
    public static class Calculation
    {


        #region Functions

        /// <summary>
        ///     Calculate Actual Cost And Actual Pmax and insert result in list of merit order
        /// </summary>
        /// <returns></returns>
        public static List<PowerPlantExtended> GetActualCostAndPMax(PayLoad payLoad)
        {
            var powerPlantMeritOrder = new List<PowerPlantExtended>();

            //To Get The Actual Cost of Euro/MWh    ==> Given x (Euro/MWh)/Efficiency
            //To Get The Actual PMax of windTurbine ==> (PMax * Wind % )/ 100
            foreach (var powerPlant in payLoad.PowerPlants)
                if (powerPlant.Type == PowerPlantTypeEnum.GasFired)
                {
                    powerPlant.ActualFuelCostPerMWh = Math.Round(payLoad.Fuels.GasPrice / powerPlant.Efficiency, 2);
                    powerPlant.ActualPMax = powerPlant.PMax;
                }
                else if (powerPlant.Type == PowerPlantTypeEnum.TurboJet)
                {
                    powerPlant.ActualFuelCostPerMWh =
                        Math.Round(payLoad.Fuels.KerosinePrice / powerPlant.Efficiency, 2);
                    powerPlant.ActualPMax = powerPlant.PMax;
                }
                else if (powerPlant.Type == PowerPlantTypeEnum.WindTurbine)
                {
                    powerPlant.ActualFuelCostPerMWh = 0;
                    powerPlant.ActualPMax = Math.Round(powerPlant.PMax * payLoad.Fuels.WindPercentage / 100, 2);
                }

            //1- Without Cost WindTurbine then order descending by ActualPMax
            var zeroCostPowerPlant =
                payLoad.PowerPlants.Where(pp => pp.Type == PowerPlantTypeEnum.WindTurbine).ToList();
            var zeroCostPowerPlantByDesc = zeroCostPowerPlant.OrderByDescending(x => x.ActualPMax);
            powerPlantMeritOrder.AddRange(zeroCostPowerPlantByDesc);

            //2- With Cost Gas And TurbineJet by ascending ActualFuelCostPerMWh
            var withCostPowerPlant = payLoad.PowerPlants.Where(pp => pp.Type == PowerPlantTypeEnum.GasFired ||
                                                                     pp.Type == PowerPlantTypeEnum.TurboJet).ToList();
            var withCostPowerPlantByAsc= withCostPowerPlant.OrderBy(x => x.ActualFuelCostPerMWh);
            powerPlantMeritOrder.AddRange(withCostPowerPlantByAsc);

            return powerPlantMeritOrder;
        }

        /// <summary>
        ///     Get the feasible combinations, by checking if Load is between the PMin and PMax Range
        /// </summary>
        /// <param name="payLoad"></param>
        /// <param name="powerPlantMeritOrder"></param>
        /// <returns></returns>
        public static List<int> GetFeasibleCombinations(PayLoad payLoad, List<PowerPlantExtended> powerPlantMeritOrder)
        {
            /*
            /// Iterate based on the number of combinations and each step we add the next power plant in the merit order
            /// Calculate the Total PMax and PMin in Each Iteration to check which one is feasible
            */
            var numberOfCombinations = powerPlantMeritOrder.Count;
            var feasibleCombinations = new List<int>();
            for (var counter = 1; counter <= numberOfCombinations; counter++)
            {
                var lstOfPowerPlantsTaken = new List<PowerPlantExtended>();
                lstOfPowerPlantsTaken.AddRange(powerPlantMeritOrder.GetRange(0, counter).ToList());

                var totalPMin = lstOfPowerPlantsTaken.Sum(s => s.PMin);
                var totalPMax = lstOfPowerPlantsTaken.Sum(s => s.ActualPMax);

                if (payLoad.Load < totalPMin || payLoad.Load > totalPMax)
                    continue; //Not Feasible
                feasibleCombinations.Add(counter); //Add to Feasible List of Combinations
            }

            return feasibleCombinations;
        }

        public static Combination CalculateLoad(PayLoad payLoad
            , List<PowerPlantExtended> powerPlants
            , Combination combination
            , List<PowerPlantExtended> powerPlantsInFeasibleCombination = null)
        {
            List<PowerPlantExtended> sumOfMinPowerInNextPowerPlants;
            if (powerPlantsInFeasibleCombination != null)
                sumOfMinPowerInNextPowerPlants = powerPlantsInFeasibleCombination;
            else
                sumOfMinPowerInNextPowerPlants = powerPlants;

            var currentIndex = 1;
            foreach (var pp in powerPlants)
                if (combination.RemainingLoad > 0)
                {
                    var productionResult = new ProductionResult();
                    productionResult.PowerPlantName = pp.Name;

                    double sumOfNextMinimumPower = 0;
                    if (currentIndex <= powerPlants.Count)
                        sumOfNextMinimumPower = sumOfMinPowerInNextPowerPlants
                            .GetRange(currentIndex, sumOfMinPowerInNextPowerPlants.Count - currentIndex).ToList()
                            .Sum(x => x.PMin);

                    var actualRemaining = combination.RemainingLoad - sumOfNextMinimumPower;
                    if (actualRemaining <= pp.ActualPMax && actualRemaining >= pp.PMin && actualRemaining > 0)
                    {
                        combination.TotalPowerProduction += actualRemaining;
                        combination.TotalCost += actualRemaining * pp.ActualFuelCostPerMWh;
                        productionResult.Production = Math.Round(actualRemaining, 1);
                        combination.RemainingLoad += -actualRemaining;
                    }
                    else if (actualRemaining > pp.ActualPMax && actualRemaining > 0)
                    {
                        combination.TotalPowerProduction += pp.ActualPMax;
                        combination.TotalCost += pp.ActualPMax * pp.ActualFuelCostPerMWh;
                        productionResult.Production = pp.ActualPMax;
                        combination.RemainingLoad += -pp.ActualPMax;
                    }
                    else
                    {
                        currentIndex += 1;
                        continue;
                    }

                    currentIndex += 1;
                    combination.ProductionResults.Add(productionResult);
                }
                else
                {
                    break;
                }

            return combination;
        }

        public static Combination GenerateRecordForUnusedPowerPlants(Combination combination,
            List<PowerPlantExtended> powerPlantsInPayLoad)
        {
            //Remaining Power Plant Not Used
            foreach (var pp in powerPlantsInPayLoad)
                if (!combination.ProductionResults.Exists(x => x.PowerPlantName == pp.Name))
                {
                    var productionResult = new ProductionResult();
                    productionResult.PowerPlantName = pp.Name;
                    productionResult.Production = 0;
                    combination.ProductionResults.Add(productionResult);
                }

            return combination;
        }

        #endregion
    }
}