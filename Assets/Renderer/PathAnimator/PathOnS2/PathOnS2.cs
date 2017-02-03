using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathOnS2 {
    public List<SphericalCoordinate> Points = new List<SphericalCoordinate>();
    public void setPath(float layers, float index) {
        
         /*
* HORIZONTAL CIRCLES ON S2*/
        index+=2;
        //increment index by 2 to avoid dividing pi by 0 or 1.
        float theta_scale = .11f;
        int i = 0;
        float sizeValue = ((2f*( Mathf.PI)) / theta_scale);
        int size = (int)sizeValue;
        float phi = (Mathf.PI / index);

        for (float theta = 0f; i < size; theta += theta_scale) {
            SphericalCoordinate SphereCoordinate = new SphericalCoordinate(1,theta, phi);

            Points.Add(SphereCoordinate);
            i++;
        }

        /*  SPIRAL ON S2
        float sizeVal = 1 / .02f;
        int size = (int)sizeVal;

        int i = 0;


        for (float t = .02f; t < 1-0.02f; t += .02f)
        {
            Points.Add(new SphericalCoordinate(1, t * layers * 2 * Mathf.PI, t * Mathf.PI));
            //_LineDrawer.SetPosition(i, SphereCoordinate.ToCartesian());
            i++;
        }
        */
    }

    public void rotatePath(Matrix4x4 rotationMatrix) {

    }

    public List<Fibre> Fibration() {
        List<Fibre> list = new List<Fibre>();
        foreach (Point3D Point in Points) {
            Fibre fib = Point.toFibre();
            list.Add(fib);
        }
        return list;
    }
}
