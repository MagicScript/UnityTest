using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChooserLogic : MonoBehaviour
{
    public delegate void OptionChosen(object option);
    public Button baseButton;

    private OptionChosen callback_;

    public void Show(string message, object[] options, bool cancel, OptionChosen choose)
    {
        Transform optionsPanel = this.transform.FindChild("OptionsPanel");
        for(int i = optionsPanel.transform.childCount-1; i >= 0; --i)
        {
            Destroy(optionsPanel.transform.GetChild(i).gameObject);
        }

        this.transform.FindChild("TitleText").GetComponent<Text>().text = message;
        callback_ = choose;

        for(int i = 0; i < options.Length; ++i)
        {
            Button button = (Button)Instantiate(baseButton);
            button.transform.SetParent(optionsPanel);
            var option = options[i];
            button.onClick.AddListener(() => { OnButtonClick(option); });

            button.transform.FindChild("Text").GetComponent<Text>().text = options[i].ToString();
        }

        if(cancel)
        {
            Button button = (Button)Instantiate(baseButton);
            button.transform.SetParent(optionsPanel);
            button.onClick.AddListener(() => { OnButtonClick(null); });

            button.transform.FindChild("Text").GetComponent<Text>().text = "Cancel";
        }

        this.gameObject.SetActive(true);
    }

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnButtonClick(object option)
    {
        if(callback_ != null)
        {
            callback_(option);
        }
        this.gameObject.SetActive(false);
    }
}
