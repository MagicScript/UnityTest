using UnityEngine;
using System.Collections;

public class Unit
{
	public string Name {get; set;}
    public int MovementPoints { get; private set; }
    public int ProductionRequired { get; private set; }

    public static Unit Warrior = new Unit() { Name = "Warrior" };

    public Unit()
    {
        MovementPoints = 1;
        ProductionRequired = 1;
    }
}
