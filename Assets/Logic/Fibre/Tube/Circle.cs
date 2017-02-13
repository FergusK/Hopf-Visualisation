using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {


    LineRenderer lr;

    Vector3[] tubeCircle;
    Vector3[] colliderCircle;
    bool drawn = false;
    int ve = -1;

        // Use this for initialization
    void Start () {
    }

	// Update is called once per frame
	public void draw (Vector3 p1, Vector3 p2, Vector3 centre)
    {
        //lr = GetComponent<LineRenderer>();

        float ThetaScale = 0.1f;
        float Theta = ThetaScale;
        
        float sizeValue = ((2f * (Mathf.PI)) / ThetaScale);
        int Size = (int)sizeValue;
        tubeCircle = new Vector3[Size];

        //lr.SetVertexCount(Size);
        //lr.numPositions = Size;
        //lr.SetWidth(0.01f, 0.01f);

        for (int i = 0; i < Size; i++)
        {
            Theta = Theta + (Mathf.PI * ThetaScale);
            tubeCircle[i] = centre + (p1 *.5f* Mathf.Cos(Theta)) + (p2 *.5f* Mathf.Sin(Theta));
            //lr.SetPosition(i, centre + (p1 * 0.1f * Mathf.Cos(Theta)) + (p2 * 0.1f * Mathf.Sin(Theta)));
        }
        //lr.SetPositions(tubeCircle);
        drawn = true;
    }

    public void drawCollider(Vector3 p1, Vector3 p2, Vector3 centre)
    {
        //lr = GetComponent<LineRenderer>();

        float ThetaScale = Mathf.PI/5;
        float Theta = ThetaScale;
        int Size = 11;

        colliderCircle = new Vector3[Size];

        //lr.SetVertexCount(Size);
        //lr.numPositions = Size;
        //lr.SetWidth(0.01f, 0.01f);

        for (int i = 0; i < Size; i++)
        {
            Theta = Theta + ThetaScale;
            colliderCircle[i] = centre + (p1 * .8f * Mathf.Cos(Theta)) + (p2 * .8f * Mathf.Sin(Theta));
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
