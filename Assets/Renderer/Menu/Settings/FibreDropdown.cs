using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FibreDropdown : MonoBehaviour {
    Dropdown fibreDrop;
	// Use this for initialization
	void Start () {
        Debug.Log("hello");
        fibreDrop = GetComponent<Dropdown>();
        fibreDrop.captionText.text = Settings.default_colour;
        List<Dropdown.OptionData> list = fibreDrop.options;

        int valIndex = 0;
        foreach (Dropdown.OptionData data in list)
        {
            if (data.text.Equals(Settings.default_colour))
            {
                fibreDrop.value = valIndex;
                return;
            }
            valIndex++;
        }
    }

    public void updateText() {
        Settings.default_colour = fibreDrop.captionText.text;
        Debug.Log("Colour: " + Settings.default_colour);
        List<Dropdown.OptionData> list = fibreDrop.options;

        int valIndex = 0;
        foreach (Dropdown.OptionData data in list)
        {
            if (data.text.Equals(Settings.default_colour))
            {
                fibreDrop.value = valIndex;
                return;
            }
            valIndex++;
        }
    }
}
