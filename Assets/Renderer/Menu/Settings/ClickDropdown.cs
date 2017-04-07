using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickDropdown : MonoBehaviour
{
    Dropdown clickDrop;
    // Use this for initialization
    void Start()
    {
        clickDrop = GetComponent<Dropdown>();
        clickDrop.captionText.text = Settings.default_click_colour;
        List<Dropdown.OptionData> list = clickDrop.options;

        int valIndex = 0;
        foreach (Dropdown.OptionData data in list) {
            if (data.text.Equals(Settings.default_click_colour)) {
                clickDrop.value = valIndex;
                return;
            }
            valIndex++;
        }
    }

    public void updateText()
    {
        Settings.default_click_colour = clickDrop.captionText.text;
        Debug.Log("Colour CLick: " + Settings.default_click_colour);

        List<Dropdown.OptionData> list = clickDrop.options;
        int valIndex = 0;
        foreach (Dropdown.OptionData data in list)
        {
            if (data.text.Equals(Settings.default_click_colour))
            {
                clickDrop.value = valIndex;
                return;
            }
            valIndex++;
        }
    }
}