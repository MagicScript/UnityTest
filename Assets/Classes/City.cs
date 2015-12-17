using UnityEngine;
using System.Collections;

public class City
{
	private World world_;
	public int X { get; private set;}
	public int Y { get;private set;}
	public string Name { get; set; }

	private int armyCount = 1;

	public City(World world, int x, int y)
	{
		world_ = world;
		X = x;
		Y = y;
	}

	public void PerformProduction()
	{
		Army inCityArmy = world_.GetArmyAt(X, Y);
		if(inCityArmy == null)
		{
			inCityArmy = world_.AddArmy(X, Y, string.Format("Army of {0}[{1}]", Name, armyCount));
			++armyCount;
		}
		Debug.Log("creating warrior");
		inCityArmy.AddUnits(Unit.Warrior, 1);
	}
}
