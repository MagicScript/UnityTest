using UnityEngine;
using System.Collections.Generic;

public class Army
{
	public delegate void ArmyChange(Army army);
	public event ArmyChange OnArmyChanged;

	private World world_;
	public int X { get; private set;}
	public int Y { get;private set;}
	public string Name { get; set; }

	private Dictionary<Unit, int> unitCounts_ = new Dictionary<Unit, int>();

	public IEnumerable<KeyValuePair<Unit, int>> Units
	{
		get { return unitCounts_; }
	}

	public Army(World world, int x, int y)
	{
		world_ = world;
		X = x;
		Y = y;
	}

	public void AddUnits(Unit unit, int count)
	{
		if(unitCounts_.ContainsKey(unit))
			unitCounts_[unit] += count;
		else
			unitCounts_[unit] = count;

		if(OnArmyChanged != null)
			OnArmyChanged(this);
	}
}
