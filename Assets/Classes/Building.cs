using System;
using System.Collections.Generic;

public class IBuilding
{
    public string Name { get; protected set; }
    public int ProductionRequired { get; protected set; }

    public virtual int GetProduction(Production what)
    {
        return 0;
    }

    public virtual int GetFood()
    {
        return 0;
    }
}

public class Field : IBuilding
{
    public Field()
    {
        Name = "Field";
        ProductionRequired = 5;
    }
}

public class Barracks : IBuilding
{
    public Barracks()
    {
        Name = "Barracks";
        ProductionRequired = 15;
    }

    public override int GetProduction(Production what)
    {
        if(what is UnitProduction)
        {
            return 2;
        }
        return 0;
    }
}
