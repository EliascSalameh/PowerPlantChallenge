using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Engine.Entities;
using REST.Engine.Entities.ExtendedEntities;
using REST.Engine.Enum;
using REST.Engine.Interface;

namespace REST.Engine
{
    public class PowerPlantBasedOnMeritOrder : IPowerPlantStrategy
    {

        #region Functions

        public Task<Combination> ExecuteProcess(PayLoad payLoad)
        {
            Combination combination = new();
            //0 
            Validation.ValidatePayload(payLoad);
            //1
            var powerPlantMeritOrder = Calculation.GetActualCostAndPMax(payLoad);
            //2
            var feasibleCombinations = Calculation.GetFeasibleCombinations(payLoad, powerPlantMeritOrder);

            if (feasibleCombinations.Count == 0)
                throw new Exception("No Feasible Combination");
            //3
            foreach (var feasibleCombination in feasibleCombinations)
            {
                combination.RemainingLoad = payLoad.Load;
                combination.TotalCost = 0;
                combination.TotalPowerProduction = 0;
                combination.ProductionResults = new List<ProductionResult>();

                var powerPlantsInFeasibleCombination = new List<PowerPlantExtended>();
                powerPlantsInFeasibleCombination.AddRange(
                    powerPlantMeritOrder.GetRange(0, feasibleCombination).ToList());

                //Power Plant without Cost
                var withoutCostPowerPlants = powerPlantsInFeasibleCombination
                    .Where(x => x.Type == PowerPlantTypeEnum.WindTurbine).ToList();
                //Power Plant with Cost
                var withCostPowerPlants = powerPlantsInFeasibleCombination.Where(x =>
                    x.Type == PowerPlantTypeEnum.GasFired ||
                    x.Type == PowerPlantTypeEnum.TurboJet).ToList();
                if (combination.RemainingLoad > 0)
                {
                    combination = Calculation.CalculateLoad(payLoad, withoutCostPowerPlants, combination,
                        powerPlantsInFeasibleCombination);
                    combination = Calculation.CalculateLoad(payLoad, withCostPowerPlants, combination);
                    combination = Calculation.GenerateRecordForUnusedPowerPlants(combination, payLoad.PowerPlants);
                }

                if (combination.RemainingLoad == 0)
                    break;
            }

            //4
            combination.TotalCost = Math.Round(combination.TotalCost, 2);
            return Task.FromResult(combination);
        }

        #endregion

        #region Properties


        #endregion
    }
}