using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIController : MonoBehaviour
{
	public GameObject cityPanel;
	public GameObject armyPanel;

    private object selected;

	public static UIController Current { get; private set; }

	// Use this for initialization
	void Start ()
	{
		Current = this;
		cityPanel.SetActive(false);
		armyPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public void SelectNothing()
	{
		cityPanel.SetActive(false);
		armyPanel.SetActive(false);
	}


	public void Select(City C)
	{
		cityPanel.SetActive(true);
		armyPanel.SetActive(false);
		ChangeText(cityPanel, "CityNameText", C.Name);

        selected = C;
    }
	
	
	public void Select(Army A)
	{
		cityPanel.SetActive(false);
		armyPanel.SetActive(true);
		ChangeText(armyPanel, "ArmyNameText", A.Name);

		string[] parts = A.Units.Select(p=> string.Format("{0} {1}", p.Value, p.Key.Name)).ToArray();
		ChangeText(armyPanel, "ArmyCountText", string.Join ("\n", parts));
        ChangeText(armyPanel, "ArmyMovementText", string.Format("{0} Movement Left", A.MovementRemaining));

        selected = A;
    }


	public void EndTurnButtonClick()
	{
		Debug.Log ("Ending Turn");
		WorldController.Current.World.PerformEndTurn();
    }

    private Tile[] displayPath_ = null;

    public void OnDrawGizmos()
    {
        if (displayPath_ != null)
        {
            for (int i = 1; i < displayPath_.Length; ++i)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(WorldController.Current.GetPositionForCell(displayPath_[i - 1]) + new Vector3(0.0f, 0.0f, -1.0f), WorldController.Current.GetPositionForCell(displayPath_[i]) + new Vector3(0.0f, 0.0f, -1.0f));
            }
        }
    }


    public void ShowPathTo(Vector3 toWorld)
    {
        if (armyPanel.activeSelf && selected is Army)
        {
            World world = WorldController.Current.World;

            Army selectedArmy = (Army)selected;
            Tile start = world.GetCellAt(selectedArmy.X, selectedArmy.Y);
            Tile target = WorldController.Current.GetClosestTile(toWorld);

            displayPath_ = WorldController.Current.World.FindPath(start, target);
            Debug.LogFormat("Found path: {0}", string.Join(", ", displayPath_.Select(t => string.Format("({0},{1})", t.X, t.Y)).ToArray()));
        }
    }


    public Tile ExecutePathTo(Vector3 toWorld)
    {
        if (armyPanel.activeSelf && selected is Army && displayPath_ != null)
        {
            Army selectedArmy = (Army)selected;
            Debug.LogFormat("Executing path: {0}", string.Join(", ", displayPath_.Select(t => string.Format("({0},{1})", t.X, t.Y)).ToArray()));
            int curr = 1;
            while(curr < displayPath_.Length && selectedArmy.MovementRemaining >= 1)
            {
                Debug.LogFormat("Moving from ({0},{1}) to ({2},{3})", selectedArmy.X, selectedArmy.Y, displayPath_[curr].X, displayPath_[curr].Y);
                selectedArmy.MoveTo(displayPath_[curr++]);
            }
            Tile ret = displayPath_[curr - 1];
            displayPath_ = null;
            ChangeText(armyPanel, "ArmyMovementText", string.Format("{0} Movement Left", selectedArmy.MovementRemaining));
            return ret;
        }

        throw new InvalidOperationException();
    }

    private void ChangeText(GameObject obj, string objName, string text)
	{
		GameObject titleText = obj.transform.FindChild(objName).gameObject;
		Text textComponent = titleText.GetComponent<Text>();
		textComponent.text = text;
	}
	
	private void ChangeText(Transform transform, string objName, string text)
	{
		GameObject titleText = transform.FindChild(objName).gameObject;
		Text textComponent = titleText.GetComponent<Text>();
		textComponent.text = text;
	}
}
