using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public UnitManager unitManager = new UnitManager();

    private static GameController current_;
    public static GameController Current
    {
        get { return current_; }
    }

    // Use this for initialization
    void Start ()
    {
        if(current_ != null)
        {
            Debug.LogError("More than one GameController");
        }
        current_ = this;

        TextAsset unitParts = (TextAsset)Resources.Load("unit_parts");
        unitManager.LoadFromXml(unitParts.text);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
