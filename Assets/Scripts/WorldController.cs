using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour
{
	public GameObject cityPanel;
	public Transform canvas;
	public Sprite baseSprite;
	public Sprite hutSprite;
	public Sprite armySprite;

	public World World { get; private set; }

	private static WorldController current_;
	public static WorldController Current
	{
		get { return current_; }
	}

    private Dictionary<Army, GameObject> armyGos = new Dictionary<Army, GameObject>();

	// Use this for initialization
	void Start ()
	{		
		Debug.Assert (current_ == null);
		current_ = this;

		World = new World ();
		World.OnCityAdd += HandleOnCityAdd;
		World.OnArmyAdd += HandleOnArmyAdd;

		GenerateMapCells ();

		foreach (var city in World.Cities)
		{
			HandleOnCityAdd(city);
		}
		
		foreach (var army in World.Armies)
		{
			HandleOnArmyAdd(army);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void GenerateMapCells()
	{
		for(int y = 0; y < World.Height; ++y)
		{
			for(int x = 0; x < World.Width; ++x)
			{
				MakeCell(x,y);
			}
		}
	}

	private void MakeCell(int x, int y)
	{
		Tile cell = World.GetCellAt (x, y);

		var cellTerrain = CreateCellGO ("terrain", "Terrain", x, y);
		OnTypeChanged (cell, cellTerrain);
		cell.OnLandTypeChanged += (Tile C)=>{OnTypeChanged(C, cellTerrain);};

		var cellImprovement = CreateCellGO ("improvement", "Improvements", x, y);
		OnImprovementChanged (cell, cellImprovement);
		cell.OnImprovementChanged += (Tile C)=>{OnImprovementChanged(C, cellImprovement);};
	}

	GameObject CreateCellGO (string baseName, string layer, int x, int y)
	{
		string name = string.Format ("{0}_{1}_{2}", baseName, x, y);
		GameObject cellTerrain = new GameObject (name, typeof(SpriteRenderer));
		cellTerrain.transform.position = GetPositionForCell(x, y);
		SpriteRenderer cellSr = cellTerrain.GetComponent<SpriteRenderer> ();
		cellSr.sortingLayerName = layer;
		cellTerrain.transform.parent = this.transform;
		return cellTerrain;
	}

	private void OnTypeChanged(Tile cell, GameObject cellGo)
	{
		SpriteRenderer cellSr = cellGo.GetComponent<SpriteRenderer> ();
		cellSr.sprite = baseSprite;
		if (cell.Type == LandType.Plain)
			cellSr.color = new Color (1.0f, 0.8f, 0.1f);
		else
			cellSr.color = new Color (0.0f, 0.0f, 0.9f);
	}	
	
	private void OnImprovementChanged(Tile cell, GameObject cellGo)
	{
		SpriteRenderer cellSr = cellGo.GetComponent<SpriteRenderer> ();
		if (cell.Improvement == Improvement.None)
			cellSr.sprite = null;
	}

	
	private void HandleOnCityAdd (City newCity)
	{
		var cellCity = CreateCellGO ("City", "Cities", newCity.X, newCity.Y);
		
		SpriteRenderer cellSr = cellCity.GetComponent<SpriteRenderer> ();
		cellSr.sprite = hutSprite;

		Vector3 pos = GetPositionForCell(newCity.X, newCity.Y) * 100.0f;
		pos += new Vector3(0.0f, -50.0f, 0.0f);
		var thisCityPanel = (GameObject)GameObject.Instantiate(cityPanel, pos, Quaternion.identity);
		thisCityPanel.GetComponentInChildren<UnityEngine.UI.Text>().text = newCity.Name;
		RectTransform myTransform = (RectTransform)thisCityPanel.transform;
		myTransform.SetParent(canvas, false);
	}
	
	private void HandleOnArmyAdd (Army newArmy)
	{
		var cellArmy = CreateCellGO ("Army", "Armies", newArmy.X, newArmy.Y);
        armyGos.Add(newArmy, cellArmy);

        SpriteRenderer cellSr = cellArmy.GetComponent<SpriteRenderer> ();
		cellSr.sprite = armySprite;

        newArmy.OnArmyMoved += OnArmyMoved;
    }

    private void OnArmyMoved(Army army)
    {
        var cellArmy = armyGos[army];
        cellArmy.transform.position = GetPositionForCell(army.X, army.Y);
    }

    public Tile GetClosestTile(Vector3 position)
	{
		return World.GetClosestTile(position);
	}


	public City GetCityAt(int x, int y)
	{
		return World.GetCityAt (x, y);
    }


    public Vector3 GetPositionForCell(Tile t)
    {
        return new Vector3(t.X, t.Y, 0.0f);
    }


    public Vector3 GetPositionForCell(int x, int y)
	{
		return new Vector3 (x, y, 0.0f);
	}

	public bool IsInWorld(Vector3 pos)
	{
		if(pos.x < -0.5f || pos.y < -0.5f)
			return false;

		if(pos.x >= World.Width - 0.5f)
			return false;
		
		if(pos.y >= World.Height - 0.5f)
			return false;

		return true;
	}
}
