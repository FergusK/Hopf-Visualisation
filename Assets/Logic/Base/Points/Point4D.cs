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
        Point3D p = new Point3D(10 * y / (w - 1), 10 * x / (w - 1), 10 * z / (w - 1));
        p.quaternionVector = new Vector4(x, y, z, w);
        return p;
    }
}
