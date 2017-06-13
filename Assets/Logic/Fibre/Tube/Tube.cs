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
    public int tubeCount, fibreCount;

    //Mesh properties
    MeshFilter filter;
    public Mesh mf;
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
        Debug.Log("Points 0 index : " + points[0].toString() + " Length : " + points.Count + " Points Length : " + Points.Count);
        fibreCount = points.Count;
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

        //float cosQPR = (Mathf.Pow(PQlength, 2) + Mathf.Pow(PRlength, 2) - Mathf.Pow(QRlength, 2)) / (2 * PQlength * PRlength);
        float cosQPR = Vector3.Dot(PQ, PR) / (PQlength*PRlength);
        /*
        print("COS: " + cosQPR);
        */

        float PD = cosQPR * PRlength;
        float CD = Mathf.Pow((Mathf.Pow(PRlength, 2) - Mathf.Pow(PD, 2)), 0.5f);
        Vector3 D = new Vector3(P.x + PD * PQnorm.x, P.y + PD * PQnorm.y, P.z + PD * PQnorm.z);

        Vector3 DC = new Vector3((R.x - D.x) / CD, (R.y - D.y) / CD, (R.z - D.z) / CD);

        float sinBAC = Mathf.Pow((1 - Mathf.Pow(cosQPR, 2)), 0.5f);
        //float sinBAC = ;
        /*
        print("SIN: "+sinBAC);
        */

        float radius = QRlength / (2 * sinBAC);

        Vector3 E = new Vector3(PQlength / 2, Mathf.Pow(Mathf.Pow(radius, 2) - Mathf.Pow((PQlength / 2), 2), 0.5f), 0);
        Vector3 centre = new Vector3(P.x + E.x * PQ.x + E.y * DC.x,
                                     P.y + E.x * PQ.y + E.y * DC.y, 
                                     P.z + E.x * PQ.z + E.y * DC.z
                                     );

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
        int colSize = Settings.Size;
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
            
            //draw and return vertices for tubeCircle
            Circle circle = new Circle();
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
        //filter.sharedMesh = mf;
        //Colouring method
        if (Settings.default_colour.Equals("Latitude")){
            ColorLatitude();
        }else if (Settings.default_colour.Equals("Longitude")) {
            ColorLongitude();
        }else if (Settings.default_colour.Equals("Quaternion")) {
            ColorQuaternionW();
        }

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
                Vector4 P = p.quaternionVector.normalized;
                colours[j + i] = Color.HSVToRGB(Mathf.Abs(P.w), 1, 1);
            }
            j += tubeCount;
        }
        mf.colors = colours;
    }

    public void ColorLatitude()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/StandardShader");
        rend.material.color = Color.HSVToRGB(Points[0].Latitude, 1, 1);
    }

    public void ColorLongitude()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/StandardShader");
        rend.material.color = Color.HSVToRGB(Points[0].Longitude, 1, 1);
    }

    public void ColorClick(float colour)
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material = rend.materials[0];

        //float w = (Points[index].quaternionVector.normalized.w);

        print("W value of clicked quaternion : " + colour);
        rend.material.color = Color.HSVToRGB(Mathf.Abs(colour), 1, 1);
    }

    public void ColorNear(int index) {
        int vIndex = index * tubeCount;
        
        for (int i = 0; i < tubeCount; i++) {
            mf.colors[vIndex + i] = Color.white;
        }
    }

    public void ColorDistanceClick(Vector4 Q)
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/StandardShader");
        rend.material.color = Color.white;

        Color[] colours = new Color[vertices.Length];
        int j = 0;
        foreach (Point3D p in Points)
        {
            Vector4 P = p.quaternionVector;
            float d_PQ = (2 * Mathf.Acos(Vector4.Dot(P, Q))) / (2 * Mathf.PI);
            for (int i = 0; i < tubeCount; i++)
            {
                colours[j + i] = Color.HSVToRGB(d_PQ, 1, 1);
            }
            j += tubeCount;
        }
        mf.colors = colours;
    }

    public void ColourDistanceClickFromBase(Vector3 Q) {

        Color[] colours = new Color[vertices.Length];
        int j = 0;

        foreach (Point3D p in Points)
        {
            Vector3 P = p.toVector3();
            float d_PQ = (Mathf.Acos(Vector3.Dot(P, Q))) / (2 * Mathf.PI);
            for (int i = 0; i < tubeCount; i++)
            {
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
        /*    
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int index = triangles[hit.triangleIndex] / tubeCount;
                Vector4 Q = hit.collider.gameObject.GetComponent<Tube>().Points[index].quaternionVector;

                Debug.Log(
                    "Mouse Down Hit the following object: " +
                    hit.collider.name + " Index : " + index +
                    " Quaternion: (" + Q.x + ", " + Q.y + ", " + Q.z + ", " + Q.w + ")" +
                    " Collision point : " + hit.point
                    );

                //if (index > fibreCount) {
                    //print(" WELL SOMETHING IS WRONG THEN : " + index);
                //}

                //print("The trouble maker index : " + index);

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
                    float colour = Q.normalized.w;
                    ColorClick(colour);
                }
                //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            }
            else if (Input.GetMouseButtonDown(2))
            {
                Debug.Log(
                            "Mouse Down Hit the following object: " +
                            hit.collider.name + " Index : " + hit.triangleIndex +
                            " Quaternion: " + hit.collider.gameObject.GetComponent<Tube>().Points[hit.triangleIndex / Settings.Size].quaternionVector.ToString()
                         );

                int index = hit.triangleIndex / Settings.Size;

                ColorClick(index);
            }
            else
            {
                Debug.Log("Nothing was hit!");
                //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            }
        }
        */
    }
}
