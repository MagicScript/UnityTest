using System.Collections.Generic;
using System.Linq;
using System;

public class City
{
	private World world_;
	public int X { get; private set;}
	public int Y { get; private set;}
	public string Name { get; set; }

    public Production CurrentProduction { get; private set; }
    public int NextProduce
    {
        get { return 1 + buildings_.Sum(b => b.GetProduction(CurrentProduction)); }
    }

    public List<IBuilding> buildings_ = new List<IBuilding>();

    public IEnumerable<IBuilding> Buildings
    {
        get { return buildings_; }
    }

    private int productionAmount_ = 0;

	public City(World world, int x, int y)
	{
		world_ = world;
		X = x;
		Y = y;
	}

    public void ChangeProduction(Production what)
    {
        CurrentProduction = what;
    }

	public void PerformProduction()
	{
        productionAmount_ += NextProduce;

        if (CurrentProduction == null)
            return;

        if(CurrentProduction.ProductionRequired <= productionAmount_)
        {
            CurrentProduction.FinishProduction(world_, this, ref productionAmount_);
            if (!CurrentProduction.AllowMultiple)
                CurrentProduction = null;
        }
    }

    public bool EstimateProduction(out int amountCompleted, out int turnsToCompletion)
    {
        if(CurrentProduction == null)
        {
            amountCompleted = -1;
            turnsToCompletion = -1;
            return false;
        }

        int nextProduce = NextProduce;
        int nextTurnProduction = productionAmount_ + nextProduce;

        if(CurrentProduction.AllowMultiple && nextTurnProduction >= CurrentProduction.ProductionRequired)
        {
            turnsToCompletion = 1;
            amountCompleted = nextTurnProduction / CurrentProduction.ProductionRequired;
        }
        else
        {
            turnsToCompletion = (int)Math.Ceiling((CurrentProduction.ProductionRequired - productionAmount_) / (float)nextProduce);
            amountCompleted = 1;
        }
        return true;
    }

    public bool HasBuilding(IBuilding check)
    {
        return buildings_.Contains(check);
    }

    public void AddBuilding(IBuilding newBuilding)
    {
        buildings_.Add(newBuilding);
    }

    public IEnumerable<Production> GetAllowedProduction()
    {
        return Production.AllProductions.Where(p => p.CanBeBuilt(world_, this));
    }
}
