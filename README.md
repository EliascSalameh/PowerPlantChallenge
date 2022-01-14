# Power Plant

## About the application
The main purpose of this solution is to calculate how much power each of a multitude of different powerplants need 
to produce when the load is given and taking into account the cost of the underlying energy sources (gas,  kerosine)
and the Pmin and Pmax of each powerplant.

### More detail
At any moment in time, all available powerplants need to generate the power to exactly match the load.
The cost of generating power can be different for every powerplant and is dependent on external factors.
Available type of power plants:
	-[Turbojet]  powerplant that runs on kerosine
	-[Gas-fired] powerplant that runs on Gaz
	-[Windmills] powerplant that runs on wind

	Each Power plant has Pmax and Pmin power that can be produced.
	Each Power plant has an efficiency calculated in %.
	For the Windmills, a powerplant with 0 cost and the power is calculated based on wind %.

When deciding which powerplant to use, it should be based on the merit-order.
	-A merit order, is a way of ranking available sources of energy, especially electrical generation, based on ascending order of price.

## Calculation

### Calculate the merit order (start by least cost margin)
For each available powerplant we should start by:
	For Turbojet and Gas-fired calculate the Actual Fuel Cost/MWh based on the given efficiency in %
		```
			Actual Cost (Euro/MWh) = (given x Euro/MWh) /Efficiency
	    ```
	-For Windmills calculate the Actual Pmax based on the given wind Percentage
	   	```
	    Actual Pmax = (PowerPlant Pmax * WindPercentage) / 100;
	    ```
### Divide PowerPlant into 2 Groups
Once we obtained the actual numbers, we will divide the powerplants into 2 groups

	-Without Cost   | Order them in Descending order by Actual Pmax
		-Windmills
	-With Cost      | Order them in ascending order  by Actual Fuel Cost/MWh
		-Gasfired
		-Turbojet
		
and add them respectively in a list, this will be the merit Order.

### Check Feasible Combinations
The Maximum number of combinations to check is equal to the count of available PowerPlants.
We will loop through each combination and calculate the Pmin and Pmax and start eliminating 
the one that are not feasible, based on the below 
    ```
    	  Pmin<Load<Pmax
    ```	
### Calculate Total Cost of the smallest Feasible Combination
We will calculate each PowerPlant how much MWH has to generate and multiply it by the Actual Fuel Cost/MWh , and sum the totals until we reach 
the Exact given Load we will stop and return the final Result.


### Return the production result
Return List of power plants with their load generated, in that way we will be covering the given load with the minimum cost.


## Run the solution

### From Executable File (.exe)
0- To Run the application, got to the solution directory.

1- double click file REST.exe, in the following path:
```
\bin\Release\net5.0\REST.exe
```

It will open a command windows showing the status of the Web API.

2-Open Post Man.

3-Call the following link, and set the setup as shown in the below screenshots
```
http://localhost:8888/api/productionplan
```

![alt text](https://github.com/EliascSalameh/PowerPlantChallenge/blob/56a415673ed1fe75f06b392bc794b26a651d4314/REST.Hosting/Screenshots/Application%20Execution%20ScreenShots/1-PostMan%20Headers%20Tab.JPG)

![alt text](https://github.com/EliascSalameh/PowerPlantChallenge/blob/56a415673ed1fe75f06b392bc794b26a651d4314/REST.Hosting/Screenshots/Application%20Execution%20ScreenShots/2-Post%20Man%20Body%20Tab.JPG)

4-The application will then call the power plant controller and from the JSon string  (ex:`payload1.Json`,...) it will read the input and calculate the unit commitment.

5-Return the expected result in the below JSON format

![alt text](https://github.com/EliascSalameh/PowerPlantChallenge/blob/56a415673ed1fe75f06b392bc794b26a651d4314/REST.Hosting/Screenshots/Application%20Execution%20ScreenShots/3-Post%20Man%20Result.PNG)

## Extra - Add Dockerfile
Screenshots for the Docker implementation under the following path .\Screenshots\Docker ScreenShots

Thank You !

Elias Salameh


