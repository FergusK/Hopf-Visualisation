using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Point3D {

    public float x, y, z;
    public float Latitude, Longitude;


    public Point3D() { }

    public Point3D(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Point3D(Vector3 vector) {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    public void transform(Matrix m)
    {
        //multiply m by point.
        float[] newMatrix = new float[4];

        for (int i = 0; i < 4; i++)
        {
            newMatrix[i] = (m.m[i,0] * x) + (m.m[i,1] * y) + (m.m[i,2] * z) + (m.m[i,3] * 1);
        }

        float w = newMatrix[3];
        x = newMatrix[0] / w;
        y = newMatrix[1] / w;
        z = newMatrix[2] / w;
    }

    public Vector3 toVector3() {
        return new Vector3(x, y, z);
    }

    public string toString() {
        return "x: " + x + " y: " + y + " z: " + z;
    }

    public Fibre toFibre() {
        Fibre f = new Fibre(GetUnitQuaternionVectors(this));
        f.Latitude = Latitude;
        f.Longitude = Longitude;
        return f;
    }

    public SphericalCoordinate CartesianToSpherical()
    {
        SphericalCoordinate store = new SphericalCoordinate();
        CartesianToSpherical(this, out store.radius, out store.polar, out store.elevation);
        return store;
    }

    private void CartesianToSpherical(Point3D cartCoords, out float outRadius, out float outPolar, out float outElevation)
    {
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        outRadius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                        + (cartCoords.y * cartCoords.y)
                        + (cartCoords.z * cartCoords.z));
        outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            outPolar += Mathf.PI;
        outElevation = Mathf.Asin(cartCoords.y / outRadius);
    }

    private List<Point4D> GetUnitQuaternionVectors(Point3D pointOnS2) {
        //Each point on S2 has a circle on S3. Cartesian3D variable represents a point on a sphere, for which
        //this method returns a list of points on R4 for the circle on the S3 sphere.

        List<Point4D> UnitQuaternions = new List<Point4D>();
        float omega_scale = .01f;
        float sizeValue = ((2f * Mathf.PI) / omega_scale);
        float size = (int)sizeValue;
        int i = 0;
        for (float omega = omega_scale; i <= size; omega += omega_scale)
        {
            UnitQuaternions.Add(fibformula(pointOnS2, omega));
            i++;
        }
        return UnitQuaternions;
    }

    private Point4D fibformula(Point3D v, float theta) {
        float ss = 1 / Mathf.Sqrt(2 * (1 + v.z));
        return new Point4D(
            (1 + v.z) * Mathf.Cos(theta)*ss,
            (v.x * Mathf.Sin(theta) - v.y * Mathf.Cos(theta))*ss,
            (v.x * Mathf.Cos(theta) + v.y * Mathf.Sin(theta))*ss,
            (1 + v.z) * Mathf.Sin(theta)*ss
            );
    }

    public static List<Vector3> toVector3List(List<Point3D> pList) {
        List<Vector3> vList = new List<Vector3>();
        foreach (Point3D point in pList) {
            vList.Add(point.toVector3());
        }
        return vList;
    }

}
