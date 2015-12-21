using System;
using System.Collections.Generic;

public class Production
{
    public string Name { get; protected set; }
    public bool AllowMultiple { get; protected set; }
    public int ProductionRequired { get; protected set; }

    private static List<Production> allProductions_ = new List<Production>();

    static Production()
    {
        new UnitProduction(Unit.Warrior);
        new BuildingProduction(new Field());
        new BuildingProduction(new Barracks());
    }

    protected Production()
    {
        allProductions_.Add(this);
    }

    public virtual void FinishProduction(World world, City C, ref int production)
    {
        throw new NotSupportedException();
    }

    public virtual bool CanBeBuilt(World world, City C)
    {
        return true;
    }

    public override string ToString()
    {
        return Name;
    }

    public static IEnumerable<Production> AllProductions
    {
        get
        {
            return allProductions_;
        }
    }
}


public class UnitProduction : Production
{
    private Unit type_;

    public UnitProduction(Unit type)
    {
        type_ = type;
        Name = type.Name;
        AllowMultiple = true;
        ProductionRequired = type.ProductionRequired;
    }

    public override void FinishProduction(World world, City C, ref int production)
    {
        Army inCityArmy = world.GetArmyAt(C.X, C.Y);
        if (inCityArmy == null)
        {
            inCityArmy = world.AddArmy(C.X, C.Y, string.Format("Army of {0}", C.Name));
        }

        int producedCount = production / ProductionRequired;
        production -= producedCount * ProductionRequired;

        inCityArmy.AddUnits(type_, producedCount);
    }
}


public class BuildingProduction : Production
{
    private IBuilding type_;

    public BuildingProduction(IBuilding type)
    {
        type_ = type;
        Name = type.Name;
        AllowMultiple = false;
        ProductionRequired = type.ProductionRequired;
    }

    public override void FinishProduction(World world, City C, ref int production)
    {
        production -= type_.ProductionRequired;
        C.AddBuilding(type_);
    }

    public override bool CanBeBuilt(World world, City C)
    {
        return !C.HasBuilding(type_);
    }
}
