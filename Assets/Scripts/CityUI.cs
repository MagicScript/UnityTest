using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class CityUI : MonoBehaviour
{
    private City city_;
    public GameObject cityBuildingListItem;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void Initialize(City C)
    {
        city_ = C;

        if(C != null)
        {
            gameObject.SetActive(true);
            Refresh();
        }
    }


    public void Refresh()
    {
        UIController.ChangeText(transform, "CityNameText", city_.Name);

        int produceTurns;
        int produceAmount;

        if (!city_.EstimateProduction(out produceAmount, out produceTurns))
        {
            UIController.ChangeText(transform, "ConstructionText", "Production: None");
        }
        else if (produceAmount > 1)
        {
            UIController.ChangeText(transform, "ConstructionText", string.Format("Production: {0} ({1}/turn)", city_.CurrentProduction, produceAmount));
        }
        else if (produceTurns != 1)
        {
            UIController.ChangeText(transform, "ConstructionText", string.Format("Production: {0} ({1} turns)", city_.CurrentProduction, produceTurns));
        }
        else
        {
            UIController.ChangeText(transform, "ConstructionText", string.Format("Production: {0} (1 turn)", city_.CurrentProduction, produceTurns));
        }

        Transform buildingContent = transform.FindChild("BuildingList/Viewport/Content");
        for (int i = buildingContent.childCount - 1; i >= 0; --i)
        {
            Destroy(buildingContent.GetChild(i).gameObject);
        }

        foreach (var building in city_.Buildings)
        {
            GameObject textGo = Instantiate(cityBuildingListItem);
            textGo.transform.SetParent(buildingContent);
            textGo.GetComponent<Text>().text = building.Name;
        }
    }

    public void ChangeProduction_OnClick()
    {
        if (gameObject.activeSelf)
        {
            Production[] producable = city_.GetAllowedProduction().ToArray();
            UIController.Current.DisplayChooser("Select new production", producable, true, (object chosen) => { ChangeProduction(city_, chosen); });
        }
    }

    private void ChangeProduction(City C, object what)
    {
        //null means cancel.
        if (what == null)
            return;

        C.ChangeProduction(what as Production);

        Refresh();
    }
}
