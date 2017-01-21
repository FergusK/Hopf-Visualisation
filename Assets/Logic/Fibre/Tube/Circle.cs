using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {


    LineRenderer lr;

    Vector3[] tubeCircle;
    bool drawn = false;

        // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	public void draw (Vector3 p1, Vector3 p2, Vector3 centre)
    {
        lr = GetComponent<LineRenderer>();

        float ThetaScale = 0.1f;
        float Theta = ThetaScale;
        int Size = (int)((2f / ThetaScale) + 1f) + 2;
        tubeCircle = new Vector3[Size];

        lr.SetVertexCount(Size);
        lr.numPositions = Size;
        lr.SetWidth(0.01f, 0.01f);

        for (int i = 0; i < Size; i++)
        {
            Theta = Theta + (Mathf.PI * ThetaScale);
            tubeCircle[i] = centre + (p1 *.1f* Mathf.Cos(Theta)) + (p2 *.1f* Mathf.Sin(Theta));
            //lr.SetPosition(i, centre + (p1 * 0.01f * Mathf.Cos(Theta)) + (p2 * 0.01f * Mathf.Sin(Theta)));
        }
        lr.SetPositions(tubeCircle);
        drawn = true;        
    }


    private void Update()
    {
        if (!drawn) return;
        rotate();
        drawRotated();
    }

    
    private void drawRotated()
    {
        lr.SetPositions(tubeCircle);
    }
    
    private void rotate() {
        Matrix m = new Matrix();
        m.setRotationY(-Mathf.PI / 50 * 40.0f * Time.deltaTime);

        for (int i = 0; i < tubeCircle.Length; i++)
        {
            Point3D p = new Point3D(tubeCircle[i]);

            p.transform(m);
            tubeCircle[i] = p.toVector3();
        }
    }
}
