using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour {
    Slider radiusSlider;

    public void Start()
    {
        radiusSlider = GetComponent<Slider>();
        radiusSlider.value = Settings.Radius;
    }

    public void updateText(Text text)
    {
        radiusSlider = GetComponent<Slider>();
        text.text = radiusSlider.value.ToString("F2");
        Settings.Radius = float.Parse(text.text);
    }
}