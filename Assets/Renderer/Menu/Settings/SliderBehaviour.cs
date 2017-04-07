using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour {
    Slider radiusSlider;

    public void Start()
    {
        radiusSlider = GetComponent<Slider>();
        Debug.Log(radiusSlider.name);

        if (radiusSlider.name.Equals("Slider"))
        {
            radiusSlider.value = Settings.Radius;
        }
        else {
            radiusSlider.value = 0.5f;
        }
    }

    public void updateText(Text text)
    {
        radiusSlider = GetComponent<Slider>();
        text.text = radiusSlider.value.ToString("F2");
        if (radiusSlider.name.Equals("Slider"))
        {
            Settings.Radius = float.Parse(text.text);
        }
    }
}