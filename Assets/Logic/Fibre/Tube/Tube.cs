using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Tube : MonoBehaviour
{

    LineRenderer lr;
    public GameObject CircleFab;
    float tubeRadius;
    int tubeCount, fibreCount;

    //Mesh properties
    MeshFilter filter;
    private Mesh mf;

    public Vector3[] verticies;        //verticies
    int verticiesIndex = 0;
    public int[] triangles;        //triangles
    public Vector2[] normals;      //normals

    private void Awake()
    {
        filter = GetComponent<MeshFilter>();
        mf = filter.mesh;
    }

    public void temp(List<Point3D> points)
    {
        fibreCount = points.Count;

    }

    public void draw3(List<Point3D> points)
    {
        lr = GetComponent<LineRenderer>();
        float ThetaScale = 0.01f;
        float Theta = 0f;
        int Size = (int)((2f / ThetaScale) + 1f);

        lr.SetVertexCount(Size);
        //lr.SetWidth(0.05f, 0.05f);


        Vector3 a = new Vector3(-0.816497f, -0.408248f, 0.408248f);
        Vector3 b = new Vector3(0.57735f, -0.57735f, 0.57735f);
        Vector3 c = new Vector3(0, 0, 0);

        //print(Size);

        for (int i = 0; i < Size; i++)
        {
            Theta = Theta + (Mathf.PI * ThetaScale);
            lr.SetPosition(i, c + (a * 1 * Mathf.Cos(Theta)) + (b * 1 * Mathf.Sin(Theta)));
        }


    }

    /**
     * This draw subroutine will render a mesh in the shape of a tube
     * given points as an argument. It will draw the mesh relevant to
     * the points and radius of the fibre, it should help visualise the
     * fibre better form just using LineRenderer to represent in 3D
     */

    public void draw2(List<Point3D> points)
    {
        /*
        */

        lr = GetComponent<LineRenderer>();
        lr.SetWidth(0.1f, 0.1f);
        lr.numPositions = points.Count;
        lr.SetVertexCount(points.Count);
        Vector3[] arrt = new Vector3[points.Count];
        int p = 0;
        foreach (Point3D point in points)
        {
            arrt[p] = point.toVector3();
            p++;
        }
        // SetPoints(arr,,Color.white);
        lr.SetPositions(arrt);
    }

    public void draw(List<Point3D> points)
    {
        fibreCount = points.Count;


        /*
        lr = GetComponent<LineRenderer>();
        lr.SetWidth(0.1f, 0.1f);
        lr.SetVertexCount(3);
        Vector3[] arrt = new Vector3[points.Count];
        int p = 0;
        foreach (Point3D point in points)
        {
            arrt[p] = point.toVector3();
            p++;
        }
       // SetPoints(arr,,Color.white);
        lr.SetPositions(arrt);
        */

        /**
         * Calculate the centre point of the circle from the points given.
         * Also calculate the Radius of the circle.
         * The results will be used to calculate perpendicualr vectors for a
         * circle at each point
        */
        Vector3 P = points[0].toVector3(),
                Q = points[1].toVector3(),
                R = points[2].toVector3();

        //chord components
        Vector3 PQ = new Vector3((Q.x - P.x), (Q.y - P.y), (Q.z - P.z));
        Vector3 QR = new Vector3((R.x - Q.x), (R.y - Q.y), (R.z - Q.z));
        Vector3 PR = new Vector3((R.x - P.x), (R.y - P.y), (R.z - P.z));

        /*
        print("X: " + P.x + " Y: " + P.y + " Z: " + P.z);
        print("X: " + Q.x + " Y: " + Q.y + " Z: " + Q.z);
        print("X: " + R.x + " Y: " + R.y + " Z: " + R.z);
        */

        float PQlength = Mathf.Pow(Mathf.Pow(PQ.x, 2) + Mathf.Pow(PQ.y, 2) + Mathf.Pow(PQ.z, 2), 0.5f);
        float QRlength = Mathf.Pow(Mathf.Pow(QR.x, 2) + Mathf.Pow(QR.y, 2) + Mathf.Pow(QR.z, 2), 0.5f);
        float PRlength = Mathf.Pow(Mathf.Pow(PR.x, 2) + Mathf.Pow(PR.y, 2) + Mathf.Pow(PR.z, 2), 0.5f);

        //Direction cosines
        Vector3 PQnorm = new Vector3(PQ.x / PQlength, PQ.y / PQlength, PQ.z / PQlength);
        Vector3 PRnorm = new Vector3(PR.x / PQlength, PR.y / PQlength, PR.z / PQlength);

        float cosQPR = (Mathf.Pow(PQlength, 2) + Mathf.Pow(PRlength, 2) - Mathf.Pow(QRlength, 2)) / (2 * PQlength * PRlength);

        /*
        print("COS: " + cosQPR);
        */

        float PD = cosQPR * PRlength;
        float CD = Mathf.Pow((Mathf.Pow(PRlength, 2) - Mathf.Pow(PD, 2)), 0.5f);
        Vector3 D = new Vector3(P.x + PD * PQnorm.x, P.y + PD * PQnorm.y, P.z + PD * PQnorm.z);

        Vector3 DC = new Vector3((R.x - D.x) / CD, (R.y - D.y) / CD, (R.z - D.z) / CD);

        float sinBAC = Mathf.Pow((1 - Mathf.Pow(cosQPR, 2)), 0.5f);

        /*
        print("SIN: "+sinBAC);
        */

        float radius = QRlength / (2 * sinBAC);

        Vector3 E = new Vector3(PQlength / 2, Mathf.Pow(Mathf.Pow(radius, 2) - Mathf.Pow((PQlength / 2), 2), 0.5f), 0);
        Vector3 centre = new Vector3(P.x + E.x * PQ.x + E.y * DC.x, P.y + E.x * PQ.y + E.y * DC.y, P.z + E.x * PQ.z + E.y * DC.z);

        //Vector3[] array = { new Vector3(0, 0, 0), centre };
        //lr.SetPositions(array);

        //print("X: " + centre.x + " Y: " + centre.y + " Z: " + centre.z);

        //print("Radius: " + radius);

        //Vector3[] TD = new Vector3[points.Count];
        List<Vector3> arr = Point3D.toVector3List(points);

        int j = 0;
        int previous = -1;

        float ThetaScale = 0.1f;
        float sizeValue = ((2f * (Mathf.PI)) / ThetaScale);
        int Size = (int)sizeValue;

        verticies = new Vector3[points.Count * Size];
        int currentPointIndex = 0;


        foreach (Point3D point in points)
        {
            //point next to current point
            if (previous == -1)
            {
                previous = points.Count - 1;
            }
            else if (previous == points.Count)
            {
                previous = 0;
            }

            /*
             * Draw a circle, on a plane relevant to two perpendicular vectors.
             * The circle must be similar to a Villarceu circle in most areas.
             * Use the circle verticies to colllect points for the Tube Mesh. 
             **/
            //tube circle verticies

            //calculate a vector perpendicular to two vectors; Vector(radius to CurrentPoint), Vector(PreviousPoint to CurrentPoint)
            Vector3 rVector = new Vector3(point.x - centre.x, point.y - centre.y, point.z - centre.z);
            Vector3 pVector = new Vector3(point.x - arr[previous].x, point.y - arr[previous].y, point.z - arr[previous].z);

            GameObject tubeCircle = Instantiate(CircleFab, Vector3.zero, Quaternion.identity) as GameObject;

            //draw and return verticies for tubeCircle
            Circle circle = tubeCircle.GetComponent<Circle>();
            circle.draw(Vector3.Cross(rVector, pVector).normalized, rVector.normalized, point.toVector3());

            Vector3[] TubeCircleVertices = circle.get();
            tubeCount = TubeCircleVertices.Length;
            //adds the tubeCircle verticies to the array of verticies to create a mesh
            setVerticies(TubeCircleVertices);

            //increments
            currentPointIndex++;
            j++;
            previous++;
        }
        setTriangles();

        mf.vertices = verticies;
        mf.triangles = triangles;
        mf.RecalculateNormals();
        mf.RecalculateBounds();

        /*
        Vector3 MQP = new Vector3((P.x + Q.x) / 2, (P.y + Q.y) / 2, (P.z + Q.z) / 2);
        Vector3 MQR = new Vector3((Q.x + R.x) / 2, (Q.y + R.y) / 2, (Q.z + R.z) / 2);

        Vector3 t = Q - P;
        Vector3 u = R - P;
        Vector3 v = R - Q;

        Vector3 w = Vector3.Cross(t, u);
        float mag = w.magnitude;

        if (mag < 10e-14) { print("false"); return; }

        // helpers
        float iwsl2 = 1.0f / (2.0f * mag);
        float tt = Vector3.Dot(t, t);
        float uu = Vector3.Dot(u, u);

        Vector3 circCenter = P + (u * tt * Vector3.Dot(u, v) - t * uu * Vector3.Dot(t, v)) * iwsl2;
        float circRadius = Mathf.Sqrt(tt * uu * Vector3.Dot(v, v) * iwsl2 * 0.5f);

        //print("X: "+points[0].x + " Y: "+ points[1].y+" Z: "+ points[2].z);
    //draw circle rotate at angle of point to radius vectorS
      //centre
      //X: -7.120542 Y: -0.07194581 Z: -7.120541
      //points
      X: -7.121067 Y: -0.07121304 Z: -7.121067
      X: -7.171061 Y: -0.1434403 Z: -7.171061
      X: -7.221045 Y: -0.2166963 Z: -7.221045



        //  and translate to point on fibre

        //collect vertex
        //create triangles
        //normals
        */

    }

    private void setVerticies(Vector3[] arr)
    {
        int arrIndex = 0;
        for (; arrIndex < arr.Length;)
        {
            //print(arr[arrIndex].ToString());
            verticies[verticiesIndex] = arr[arrIndex];
            arrIndex++;
            verticiesIndex++;
        }
    }

    private void setTriangles()
    {
        int length = tubeCount * 40;
        triangles = new int[tubeCount * 40 * 6];
        int flag = 0;
        for (int i = 0, t = tubeCount*2; t < length; i += 6, t++)
        {
            if (flag == tubeCount - 1)
            {
                flag = 0;
                i -= 6;
            }
            else
            {
                triangles[i] = t;
                triangles[i + 1] = tubeCount + t + 1;
                triangles[i + 2] = tubeCount + t;
                triangles[i + 3] = t;
                triangles[i + 4] = t + 1;
                triangles[i + 5] = tubeCount + t + 1;
                flag++;
            }
        }

        //print(tubeCount);

        /*triangles = new int[(verticies.Length-1)*6];
        for (int i = 0; i < triangles.Length-1; i+=4)
        {
            for (int j = 0; j < (tubeCount * 6); j += 6)
            {
                triangles[i + j] = i;
                triangles[i + j + 1] = i + 1 + tubeCount;
                triangles[i + j + 2] = i + tubeCount;

                triangles[i + j + 3] = i;
                triangles[i + j + 4] = i + 1;
                triangles[i + j + 5] = i + tubeCount + 1;
                /*if (i >= triangles.Length - tubeCount - 1)
                {
                    triangles[i + j] = i;
                    triangles[i + j + 1] = ;
                    triangles[i + j + 2] = i - triangles.Length;

                    triangles[i + j + 3] = i;
                    triangles[i + j + 4] = i + 1;
                    triangles[i + j + 5] = i - triangles.Length + 1;
                }
                else
                {
                    triangles[i + j] = i;
                    triangles[i + j + 1] = i + 1 + tubeCount;
                    triangles[i + j + 2] = i + tubeCount;

                    triangles[i + j + 3] = i;
                    triangles[i + j + 4] = i + 1;
                    triangles[i + j + 5] = i + tubeCount + 1;
                }
            }
        }*/

    }

    public void clear()
    {
        mf.Clear();
        verticies = null;
        triangles = null;
        verticiesIndex = 0;
    }

    private void Update()
    {
        rotate();
    }

    public void rotate()
    {
        Matrix m = new Matrix();
        m.setRotationY((Mathf.PI / 100) * 2.0f * Time.deltaTime);

        for (int i = 0; i < verticies.Length; i++)
        {
            Point3D p = new Point3D(verticies[i]);

            p.transform(m);
            verticies[i] = p.toVector3();
        }

        mf.vertices = verticies;
        mf.RecalculateBounds();
        mf.RecalculateNormals();
    }
}
