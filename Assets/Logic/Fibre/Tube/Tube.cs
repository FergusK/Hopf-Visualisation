using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Tube : MonoBehaviour
{
    public List<Point3D> Points;

    //tube rotation
    public int rotate;
    public float rotation_speed;
    public Vector3 axis;

    //rendering
    LineRenderer lr;
    public GameObject CircleFab;
    float tubeRadius;
    int tubeCount, fibreCount;

    //Mesh properties
    MeshFilter filter;
    private Mesh mf;
    public Vector3[] vertices;        //vertices
    int verticesIndex = 0;
    public int[] triangles;        //triangles
    public Vector2[] normals;      //normals

    //Collider Mesh properties
    private MeshCollider mc;
    private Mesh colliderMesh;
    Vector3[] colVertices;
    int colliderVerticesIndex = 0;
    int[] colliderTriangles;
    int colliderTubeCount;

    private void Awake()
    {
        filter = GetComponent<MeshFilter>();
        mf = filter.mesh;
        mc = GetComponent<MeshCollider>();
        colliderMesh = new Mesh();
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

    /**
     * This draw call will render a mesh in the shape of a tube
     * given points as an argument. It will draw the mesh relevant to
     * the points and radius of the fibre, it should help visualise the
     * fibre better form just using LineRenderer to represent in 3D
     */
    public void draw(List<Point3D> points)
    {
        points.Add(points[1]);
        Points = points;
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

        //initalise Mesh variables
        int j = 0;
        int previous = -1;

       
        int Size = Settings.Size;

        vertices = new Vector3[(points.Count) * Size];
        int currentPointIndex = 0;


        //Initialise Collider Mesh variables
        int colSize = 11;
        colVertices = new Vector3[points.Count * colSize];


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
             * Use the circle vertices to colllect points for the Tube Mesh. 
             **/

            //tube circle vertices

            //calculate a vector perpendicular to two vectors; Vector(radius to CurrentPoint), Vector(PreviousPoint to CurrentPoint)
            Vector3 rVector = new Vector3(point.x - centre.x, point.y - centre.y, point.z - centre.z);
            Vector3 pVector = new Vector3(point.x - arr[previous].x, point.y - arr[previous].y, point.z - arr[previous].z);

            GameObject tubeCircle = Instantiate(CircleFab, Vector3.zero, Quaternion.identity) as GameObject;

            //draw and return vertices for tubeCircle
            Circle circle = tubeCircle.GetComponent<Circle>();
            circle.draw(Vector3.Cross(rVector, pVector).normalized, rVector.normalized, point.toVector3());

            Vector3[] TubeCircleVertices = circle.get();
            tubeCount = TubeCircleVertices.Length;
            //adds the tubeCircle vertices to the array of vertices to create a mesh
            setvertices(TubeCircleVertices);

            //Collider Mesh
            circle.drawCollider(Vector3.Cross(rVector, pVector).normalized, rVector.normalized, point.toVector3());
            Vector3[] colliderCircleVertices = circle.getCollider();
            setColliderVertices(colliderCircleVertices);
            colliderTubeCount = colliderCircleVertices.Length;

            //increments
            currentPointIndex++;
            j++;
            previous++;
        }

        //Set mesh values
        setTriangles();

        var normals = new Vector3[vertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            normals[i] = vertices[i].normalized;
        }
        mf.vertices = vertices;
        mf.normals = normals;
        mf.triangles = triangles;

        //Colouring method
        if (Settings.default_colour.Equals("Latitude")){
            ColorLatitude();
        }else if (Settings.default_colour.Equals("Longitude")) {
            ColorLongitude();
        }else if (Settings.default_colour.Equals("Quaternion")) {
            ColorQuaternionW();
        }

        // create new colors array where the colors will be created.

        /*
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
        }
        */

        // assign the array of colors to the Mesh.
        //mf.colors = colors;
        mf.RecalculateBounds();


        //Set collider mesh values
        setColliderTriangles();

        var ColliderNormals = new Vector3[colVertices.Length];
        for (var i = 0; i < colVertices.Length; i++)
        {
            ColliderNormals[i] = colVertices[i].normalized;
        }

        colliderMesh.vertices = colVertices;
        colliderMesh.triangles = colliderTriangles;
        colliderMesh.normals = ColliderNormals;

        colliderMesh.RecalculateNormals();
        mc.sharedMesh = colliderMesh;

    }

    private void setvertices(Vector3[] arr)
    {
        int arrIndex = 0;
        for (; arrIndex < arr.Length;)
        {
            //print(arr[arrIndex].ToString());
            vertices[verticesIndex] = arr[arrIndex];
            arrIndex++;
            verticesIndex++;
        }
    }

    private void setTriangles()
    {
        int length = tubeCount * (fibreCount - 1);
        triangles = new int[length * 6];
        int flag = 0;
        for (int i = 0, t = 0; t < length; i += 6, t++)
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
    }

    public void setColliderVertices(Vector3[] arr)
    {
        int arrIndex = 0;
        for (; arrIndex < arr.Length;)
        {
            //print(arr[arrIndex].ToString());
            colVertices[colliderVerticesIndex] = arr[arrIndex];
            arrIndex++;
            colliderVerticesIndex++;
        }
    }

    public void setColliderTriangles()
    {


        int length = colliderTubeCount * (fibreCount - 1);
        colliderTriangles = new int[length * 6];
        int flag = 0;
        for (int i = 0, t = 0; t < length; i += 6, t++)
        {
            if (flag == colliderTubeCount - 1)
            {
                flag = 0;
                i -= 6;
            }
            else
            {
                colliderTriangles[i] = t;
                colliderTriangles[i + 1] = colliderTubeCount + t + 1;
                colliderTriangles[i + 2] = colliderTubeCount + t;
                colliderTriangles[i + 3] = t;
                colliderTriangles[i + 4] = t + 1;
                colliderTriangles[i + 5] = colliderTubeCount + t + 1;
                flag++;
            }
        }

    }

    public void clear()
    {
        mf.Clear();
        vertices = null;
        triangles = null;
        verticesIndex = 0;

        colliderVerticesIndex = 0;

    }

    public void ColorQuaternionW()
    {
        Color[] colours = new Color[vertices.Length];
        int j = 0;
        foreach (Point3D p in Points)
        {
            for (int i = 0; i < tubeCount; i++)
            {
                Vector4 P = p.quaternionVector;
                colours[j + i] = Color.HSVToRGB(P.w, 1, 1);
            }
            j += tubeCount;
        }
        mf.colors = colours;
    }

    public void ColorLatitude()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/StandardShader");
        //print(points[0].Longitude);
        rend.material.color = Color.HSVToRGB(Points[0].Longitude, 1, 1);
    }

    public void ColorLongitude()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/StandardShader");
        //print(points[0].Longitude);
        rend.material.color = Color.HSVToRGB(Points[0].Latitude, 1, 1);
    }

    public void ColorClick(int index)
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material = rend.materials[0];

        float w = (Points[index].quaternionVector.w + 1) / 2;
        //print(points[0].Longitude);
        rend.material.color = Color.HSVToRGB(w, 1, 1);
    }

    public void ColorDistanceClick(Vector4 Q)
    {

        Color[] colours = new Color[vertices.Length];
        int j = 0;
        foreach (Point3D p in Points)
        {
            for (int i = 0; i < tubeCount; i++)
            {
                Vector4 P = p.quaternionVector;
                float d_PQ = (2 * Mathf.Acos(Mathf.Abs(Vector4.Dot(P, Q)))) / (Mathf.PI);

                colours[j + i] = Color.HSVToRGB(d_PQ, 1, 1);
            }
            j += tubeCount;
        }
        mf.colors = colours;
    }

    public void ColorDotProductClick(Vector4 Q) {
        Color[] colours = new Color[vertices.Length];
        int j = 0;
        foreach (Point3D p in Points) {
            for (int i = 0; i < tubeCount; i++) {
                float dot = Vector4.Dot(Q, p.quaternionVector);
                colours[j + i] = Color.HSVToRGB(dot, 1, 1);
            }
            j += tubeCount;
        }
        mf.colors = colours;
    }

    private void Update()
    {
        if (rotate == 1)
        {
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(0, rotation_speed, 0);  //the degrees the vertices are to be rotated, for example (0,90,0) 
            this.transform.Rotate(newRotation.eulerAngles);
        }
        else if (rotate == 2) {
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(0, -rotation_speed, 0);  //the degrees the vertices are to be rotated, for example (0,90,0) 
            this.transform.Rotate(newRotation.eulerAngles);
        }
            
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(
                    "Mouse Down Hit the following object: " +
                    hit.collider.name + " Index : " + (hit.triangleIndex / 6) / 11 +
                    " Quaternion: " + hit.collider.gameObject.GetComponent<Tube>().Points[(hit.triangleIndex / 6)].quaternionVector.ToString() +
                    " Collision point : " + hit.point
                    );
                //hit.collider.gameObject.GetComponent<Tube>().ColorClick((hit.triangleIndex) / 3);

                //Distance
                int index = (hit.triangleIndex / 6) / 11;
                Vector4 Q = hit.collider.gameObject.GetComponent<Tube>().Points[index].quaternionVector;



                //Colouring method
                if (Settings.default_click_colour.Equals("Distance"))
                {
                    ColorDistanceClick(Q);
                }
                else if (Settings.default_click_colour.Equals("Dot"))
                {
                    ColorDotProductClick(Q);
                }
                else if (Settings.default_click_colour.Equals("Quaternion"))
                {
                    ColorClick(index);
                }

                Debug.DrawRay(ray.origin, ray.direction, Color.green);

            }
            else if (Input.GetMouseButtonDown(2))
            {
                Debug.Log(
                            "Mouse Down Hit the following object: " +
                            hit.collider.name + " Index : " + hit.triangleIndex +
                            " Quaternion: " + hit.collider.gameObject.GetComponent<Tube>().Points[hit.triangleIndex / 3].quaternionVector.ToString()
                         );

                int index = hit.triangleIndex;

                ColorClick(index);

                Debug.DrawRay(ray.origin, ray.direction, Color.green);

            }
            else
            {

                Debug.Log("Nothing was hit!");
                Debug.DrawRay(ray.origin, ray.direction, Color.green);

            }
        }
    }

    /*public void rotate()
    {
        Matrix m = new Matrix();
        m.setRotationY((Mathf.PI / 11) * 1.5f * Time.deltaTime);

        for (int i = 0; i < vertices.Length; i++)
        {
            Point3D p = new Point3D(vertices[i]);

            p.transform(m);
            vertices[i] = p.toVector3();
        }

        mf.vertices = vertices;
        mf.RecalculateBounds();
    }*/
}
