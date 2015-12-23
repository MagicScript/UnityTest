using UnityEngine;
using System.Collections.Generic;

public class Unit
{
	public string Name {get; set;}
    public int MovementPoints { get; private set; }
    public int ProductionRequired { get; private set; }

    private UnitPart[] parts_;
    public IEnumerable<UnitPart> Parts
    {
        get { return parts_; }
    }

    public Unit(string name, UnitPart[] parts)
    {
        Name = name;
        parts_ = parts;
        MovementPoints = 1;
        ProductionRequired = 1;

        foreach (var p in parts_)
        {
            ProductionRequired += p.Cost;
        }
    }

    public Unit(string name, List<UnitPart> parts) : this(name, parts.ToArray())
    {
    }
}
