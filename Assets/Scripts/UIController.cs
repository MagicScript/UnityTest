using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class UIController : MonoBehaviour
{
	public GameObject cityPanel;
	public GameObject armyPanel;

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
	}
	
	
	public void Select(Army A)
	{
		cityPanel.SetActive(false);
		armyPanel.SetActive(true);
		ChangeText(armyPanel, "ArmyNameText", A.Name);

		string[] parts = A.Units.Select(p=> string.Format("{0} {1}", p.Value, p.Key.Name)).ToArray();
		ChangeText(armyPanel, "ArmyCountText", string.Join ("\n", parts));
	}


	public void EndTurnButtonClick()
	{
		Debug.Log ("Ending Turn");
		WorldController.Current.World.PerformEndTurn();
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
