using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathOnS2
{
    public List<SphericalCoordinate> Points = new List<SphericalCoordinate>();
    public float rotation;
    public int rotate;
    public float rotation_speed;
    

    /*
     * HORIZONTAL CIRCLES ON S2 
     * set Points with new values.
     * Circle path along x axis defined by phi.
     **/
    public void CirclePath(float phi, float scale, float circleWidth = (2f*Mathf.PI))
    {
        scale = (scale <= 0.01f) ? (0.01f) : ((scale < Mathf.PI*2) ? (scale) : (Mathf.PI*2));
        circleWidth = (circleWidth > 2 * Mathf.PI) ? (2 * Mathf.PI) : ((circleWidth < 0) ? (0) : (circleWidth));
        
        int i = 0;
        float sizeValue = (circleWidth / scale);
        int size = (int)sizeValue;
        size++;
        for (float theta = 0f; i < size; theta += scale)
        {
            SphericalCoordinate SphereCoordinate = new SphericalCoordinate(1, theta, phi);
            SphereCoordinate.Latitude = phi / (Mathf.PI);
            SphereCoordinate.Longitude = theta / (Mathf.PI * 2);

            Points.Add(SphereCoordinate);
            i++;
        }
    }

    public void CirclePathVertical(float theta, float scale, float circleWidth = (2f * Mathf.PI)) {
        CirclePath(theta, scale, circleWidth);
        Matrix m = new Matrix();
        m.setRotationX(90);
        rotatePath(m);
    }

    public void SpiralPath(int spins)
    {
        float scale = Mathf.PI / 60;
        /*  SPIRAL ON S2
        */
        if (spins <= 2)
        {
            scale = Mathf.PI / 60;
        }
        else if (spins <= 3 )
        {
            scale = Mathf.PI / 90;
        }
        else if (spins <= 6)
        {
            scale = Mathf.PI / 110;
        }
        else if (spins <= 8) {
            scale = Mathf.PI / 170;
        }

        float sizeVal = 1 / scale;
        int size = (int)sizeVal;

        int i = 0;


        for (float t = scale; t < 1 - scale; t += scale)
        {
            SphericalCoordinate SphereCoordinate = new SphericalCoordinate(1, t * spins * Mathf.PI, t * Mathf.PI);
            Points.Add(SphereCoordinate);
            //_LineDrawer.SetPosition(i, SphereCoordinate.ToCartesian());
            SphereCoordinate.Latitude = t;
            SphereCoordinate.Longitude = t;
            i++;
        }
    }

    public void SpiralPathHorizontal(int spins) {
        SpiralPath(spins);
        Matrix m = new Matrix();
        m.setRotationY(90);
        rotatePath(m);
    }

    public void rotatePath(Matrix rotationMatrix)
    {
        foreach (Point3D point in Points) {
            point.transform(rotationMatrix);
        }
    }

    public List<Fibre> Fibration()
    {
        List<Fibre> list = new List<Fibre>();
        foreach (Point3D Point in Points)
        {
            Fibre fib = Point.toFibre();
            fib.rotate = rotate;
            fib.rotation_speed = rotation_speed;
            list.Add(fib);
        }
        return list;
    }

    public void render() {
        
    }
}