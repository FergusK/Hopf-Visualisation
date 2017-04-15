using UnityEngine;
using System.Collections;

public class Point4D{

    private float x, y, z, w;

    public Point4D(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Point3D project()
    {
        Point3D p = new Point3D(10 * (y / (1 + w)), 10 * (x / (1 + w)), 10 * (z / (1 + w)));
        p.quaternionVector = new Vector4(x, y, z, w);
        return p;
    }
}
