using UnityEngine;
using System.Collections;

public class Circle {


    LineRenderer lr;

    Vector3[] tubeCircle;
    Vector3[] colliderCircle;
    bool drawn = false;
    int ve = -1;

	// Update is called once per frame
	public void draw (Vector3 p1, Vector3 p2, Vector3 centre)
    {
        //lr = GetComponent<LineRenderer>();

        float ThetaScale = Settings.ThetaScale;
        float Theta = ThetaScale;
        int Size = Settings.Size;
        tubeCircle = new Vector3[Size];

        //lr.SetVertexCount(Size);
        //lr.numPositions = Size;
        //lr.SetWidth(0.01f, 0.01f);

        for (int i = 0; i < Size; i++)
        {
            Theta = Theta + (2*Mathf.PI * ThetaScale);
            tubeCircle[i] = centre + (p1 *Settings.Radius * Mathf.Cos(Theta)) + (p2 * Settings.Radius * Mathf.Sin(Theta));
            //lr.SetPosition(i, centre + (p1 * 0.1f * Mathf.Cos(Theta)) + (p2 * 0.1f * Mathf.Sin(Theta)));
        }
        //lr.SetPositions(tubeCircle);
        drawn = true;
    }

    public void drawCollider(Vector3 p1, Vector3 p2, Vector3 centre)
    {
        //lr = GetComponent<LineRenderer>();

        float ThetaScale = Settings.ThetaScale;
        float Theta = ThetaScale;
        int Size = Settings.Size;

        colliderCircle = new Vector3[Size];

        //lr.SetVertexCount(Size);
        //lr.numPositions = Size;
        //lr.SetWidth(0.01f, 0.01f);

        for (int i = 0; i < Size; i++)
        {
            Theta = Theta + (2*Mathf.PI * ThetaScale);
            colliderCircle[i] = centre + (p1 * (Settings.Radius+.3f) * Mathf.Cos(Theta)) + (p2 * (Settings.Radius+.3f) * Mathf.Sin(Theta));
        }
    }

    public Vector3[] get(){
        return tubeCircle;
    }

    public Vector3[] getCollider()
    {
        return colliderCircle;
    }

    private void Update2()
    {
        if (!drawn) return;
        if(!Input.GetKey(KeyCode.F)){

            if(Input.GetKey(KeyCode.Q)){
                ve = 1;
            }else if(Input.GetKey(KeyCode.E)){
                ve = -1;
            }

            rotate(ve);
            drawRotated();
        }
    }


    private void drawRotated()
    {
        lr.SetPositions(tubeCircle);
    }

    private void rotate(int ve) {
        Matrix m = new Matrix();
        m.setRotationY((ve*Mathf.PI / 100) * 2.0f * Time.deltaTime);

        for (int i = 0; i < tubeCircle.Length; i++)
        {
            Point3D p = new Point3D(tubeCircle[i]);

            p.transform(m);
            tubeCircle[i] = p.toVector3();
        }
    }
}
