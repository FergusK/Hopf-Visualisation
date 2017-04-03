using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings{

    //Variables for drawing tube
    public static float ThetaScale = 0.1f;
    private static float sizeValue = ((2f * (Mathf.PI)) / ThetaScale);
    public static int Size = (int)sizeValue;
    public static float Radius = 0.9f;

    //collider variables
    public static int ColliderSize = 11;
    public static float ColliderThetaScale = Mathf.PI / 5;

    public static string default_colour = "Latitude";
    public static string default_click_colour = "Distance";

    //S2 input
    public static List<S2Input> S2List = new List<S2Input>();

}
