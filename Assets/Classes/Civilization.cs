using System.Collections.Generic;

public class Civilization
{
    private List<Unit> units_ = new List<Unit>();
    private List<Production> productions_ = new List<Production>();

    public IEnumerable<Production> AllProductions
    {
        get { return productions_; }
    }

    public Civilization()
    {
        AddBuilding(new Field());
        AddBuilding(new Barracks());

        AddUnit(new Unit("Warrior", new UnitPart[] { GameController.Current.unitManager.GetPart("WEAPON_CLUB") }));
    }

    public void AddUnit(Unit unit)
    {
        units_.Add(unit);
        productions_.Add(new UnitProduction(unit));
    }

    public void AddBuilding(IBuilding building)
    {
        productions_.Add(new BuildingProduction(building));
    }
}
