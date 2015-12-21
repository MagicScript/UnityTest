using System.Collections.Generic;
using System.Linq;
using System;

public class Army
{
	public delegate void ArmyChange(Army army);
	public event ArmyChange OnArmyChanged;

    public delegate void ArmyMove(Army army);
    public event ArmyMove OnArmyMoved;

    //private World world_;
	public int X { get; private set;}
	public int Y { get;private set;}
    public int MovementRemaining { get; private set; }
	public string Name { get; set; }

	private Dictionary<Unit, int> unitCounts_ = new Dictionary<Unit, int>();

	public IEnumerable<KeyValuePair<Unit, int>> Units
	{
		get { return unitCounts_; }
	}

	public Army(World world, int x, int y)
	{
		//world_ = world;
		X = x;
		Y = y;
    }

	public void AddUnits(Unit unit, int count)
	{
        if (unitCounts_.ContainsKey(unit))
            unitCounts_[unit] += count;
        else if(unitCounts_.Count == 0)
        {
            unitCounts_[unit] = count;
            MovementRemaining = unit.MovementPoints;
        }
        else
        {
            unitCounts_[unit] = count;
            MovementRemaining = Math.Min(MovementRemaining, unit.MovementPoints);
        }

		if(OnArmyChanged != null)
			OnArmyChanged(this);
	}

    public void MoveTo(Tile t)
    {
        if (Math.Abs(t.X - X) > 1 || Math.Abs(t.Y - Y) > 1 ||  MovementRemaining < 1)
            throw new InvalidOperationException();

        X = t.X;
        Y = t.Y;
        MovementRemaining -= 1;

        if (OnArmyMoved != null)
            OnArmyMoved(this);
    }

    public void EndTurn()
    {
        MovementRemaining = unitCounts_.Keys.Select(u => u.MovementPoints).Min();
    }
}
