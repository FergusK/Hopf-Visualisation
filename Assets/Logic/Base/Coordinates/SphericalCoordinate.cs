using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class SphericalCoordinate : Point3D
{
    public float radius;
    public float polar;
    public float elevation;
    public SphericalCoordinate() { }

    public SphericalCoordinate(float r, float t, float p) {
        radius=r;
        polar=t;
        elevation=p;
        //set super
        ToCartesian();
    }

    public void ToCartesian() {
        float a = radius * Mathf.Sin(elevation);
        x = a * Mathf.Cos(polar);
        z = radius * Mathf.Cos(elevation);
        y = radius * Mathf.Sin(polar) * Mathf.Sin(elevation);
    }
    

}