﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    List<S2Input> userInput;
    Text text1;
    Text text2;
    Text text3;
    Text textScale;
    Text speedText;
    Text rotationDirection;

    private void Awake()
    {
        userInput = Settings.S2List;
    }

    public void setRotation(Text direction) {
        rotationDirection = direction;
    }

    public void setText1(Text text) {
        text1 = text;
    }

    public void setText2(Text text) {
        text2 = text;
    }

    public void setText3(Text text)
    {
        text3 = text;
    }

    public void setTextScale(Text text) {
        textScale = text;
    }

    public void setSpeed(Text text) {
        speedText = text;
    }

    public void addCircleInput() {
        S2Input input = null;
        if (Transform.FindObjectsOfType<Toggle>()[2].isOn) {
            input = new S2Input("Circle path vertical", float.Parse(text2.text), float.Parse(text3.text), float.Parse(textScale.text));
        } else {
            input = new S2Input("Circle path", float.Parse(text1.text), float.Parse(text3.text), float.Parse(textScale.text));
        }

        if (Transform.FindObjectsOfType<Toggle>()[1].isOn)
        {
            input.setRotationInfo(1, float.Parse(speedText.text));
        }
        userInput.Add(input);
    }

    public void addSpiralInput(Text arg1) {
        S2Input input = null;
        if (Transform.FindObjectsOfType<Toggle>()[3].isOn)
        {
            input = new S2Input("Spiral path", int.Parse(arg1.text.Substring(0, 1).ToString()));
        }
        else {
            input = new S2Input("Spiral horizontal path", int.Parse(arg1.text.Substring(0,1).ToString()));
        }
        if (Transform.FindObjectsOfType<Toggle>()[0].isOn)
        {
            Debug.Log("Rotation set");
            string val = rotationDirection.text.ToString();
            input.setRotationInfo(val.Equals("Clockwise") ? 1 : 2, float.Parse(speedText.text));
        }
        userInput.Add(input);
    }

    public void Play() {
        Application.LoadLevel("Hopf scene");
    }

    public void Visualise() {
        Application.LoadLevel("BaseVisualise");
    }

}