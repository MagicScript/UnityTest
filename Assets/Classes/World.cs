﻿using UnityEngine;
using System.Collections.Generic;

public class World
{
	public delegate void CityAdd (City newCity);
	public event CityAdd OnCityAdd;

	public delegate void ArmyAdd (Army newArmy);
	public event ArmyAdd OnArmyAdd;

	private Tile[,] cells_;

	public int Width { get; private set; }
	public int Height { get; private set; }

	private List<City> cities_ = new List<City> ();

	private List<Army> armies_ = new List<Army>();

	public IEnumerable<City> Cities
	{
		get { return cities_; }
	}
	
	public IEnumerable<Army> Armies
	{
		get { return armies_; }
	}

	public World()
	{
		Width = 5;
		Height = 5;
		cells_ = new Tile[Width, Height];
		for(int j = 0; j < Height; ++j)
		{
			for(int i = 0; i < Width; ++i)
			{
				cells_[i,j] = new Tile( i, j );
				cells_[i,j].Type = LandType.Plain;
			}
		}

		AddCity (0, 0, "Ottawa");
		AddCity (3, 3, "Toronto");
	}

	public Tile GetCellAt(int x, int y)
	{
		return cells_[x,y];
	}
	
	public IEnumerable<Tile> GetNeighbours(Tile cell)
	{
		return GetNeighbours (cell.X, cell.Y);
	}

	private  IEnumerable<Tile> GetNeighbours(int x, int y)
	{
		if (x > 0)
			yield return cells_ [x - 1, y];
		
		if (x > 0 && y > 0)
			yield return cells_ [x - 1, y - 1];
		
		if (y > 0)
			yield return cells_ [x, y - 1];
		
		if (x < Width-1 && y > 0)
			yield return cells_ [x + 1, y - 1];
		
		if (x < Height-1)
			yield return cells_ [x + 1, y];

		if (x < Height-1 && y < Height-1)
			yield return cells_ [x + 1, y + 1];
		
		if (y < Height-1)
			yield return cells_ [x, y + 1];
		
		if (x > 0 && y < Height-1)
			yield return cells_ [x - 1, y + 1];
	}
	
	public Tile GetClosestTile(Vector3 position)
	{
		int x = Mathf.RoundToInt (position.x);
		int y = Mathf.RoundToInt (position.y);

		return cells_[x, y];
	}

	public City GetCityAt(int x, int y)
	{
		foreach (var city in cities_)
		{
			if(city.X == x && city.Y == y)
				return city;
		}
		return null;
	}
	
	public Army GetArmyAt(int x, int y)
	{
		foreach (var army in armies_)
		{
			if(army.X == x && army.Y == y)
				return army;
		}
		return null;
	}


	public City AddCity(int x, int y, string name)
	{
		Debug.LogFormat("Creating new city at {0}, {1}", x, y);
		City newCity = new City(this, x, y) { Name = name };
		cities_.Add (newCity);

		if(OnCityAdd != null)
			OnCityAdd(newCity);

		return newCity;
	}
	
	
	public Army AddArmy(int x, int y, string name)
	{
		Debug.LogFormat("Creating new army at {0}, {1}", x, y);
		Army newArmy = new Army(this, x, y) { Name = name };
		armies_.Add (newArmy);
		
		if(OnArmyAdd != null)
			OnArmyAdd(newArmy);

		return newArmy;
	}


	public void PerformEndTurn()
	{
		foreach(var city in cities_)
		{
			city.PerformProduction();
		}
	}
}