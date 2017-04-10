using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PathAnimator : MonoBehaviour
{

    public GameObject TubeFab;
    //public LineRenderer Renderer;

    List<PathOnS2> Layers;
    List<List<Fibre>> HopfLayers;
    List<List<Tube>> HF;
    int noOfLayers;

    // Use this for initialization
    void Awake()
    {
        Layers = new List<PathOnS2>();
        HopfLayers = new List<List<Fibre>>();
        HF = new List<List<Tube>>();
        //noOfLayers = Settings.S2List.Count;

        main();
    }

    void main()
    {
        Base();
        Fibration();
        Projection();
    }

    void Base()
    {
        //get menu information and translate to S2 points

        Debug.Log("outside!");
        foreach (S2Input s2layer in Settings.S2List)
        {
            Debug.Log("exists!");
            PathOnS2 path = new PathOnS2();

            if (s2layer.type.Equals("Circle path")) {
                Debug.Log("inside!");
                path.CirclePath(s2layer.FloatParam1, s2layer.FloatParam2, s2layer.FloatParam3);
            } else if (s2layer.type.Equals("Circle path vertical")) {
                path.CirclePathVertical(s2layer.FloatParam1, s2layer.FloatParam2, s2layer.FloatParam3);
            }
            else if (s2layer.type.Equals("Spiral path")) {
                path.SpiralPath(s2layer.IntParam1, s2layer.FloatParam1);
            } else if (s2layer.type.Equals("Spiral horizontal path")) {
                path.SpiralPathHorizontal(s2layer.IntParam1, s2layer.FloatParam1);
            }

            path.rotate = s2layer.rotate;
            if (s2layer.rotate == 1 || s2layer.rotate == 2) 
                path.rotation_speed = s2layer.rotation_speed;
            Layers.Add(path);
        }


        #region Base
        //for (int i = 0; i < noOfLayers; i++)
        //{
        //    PathOnS2 path = new PathOnS2();
        //    path.CirclePath(i);

        //    Layers.Add(path);
        //}
        //PathOnS2 path3 = new PathOnS2();
        //path3.CirclePath(0.4f);

        //Layers.Add(path3);
        /*
        PathOnS2 path2 = new PathOnS2();
        path2.CirclePath(1.1f, 0.11f);

        Layers.Add(path2);

        PathOnS2 path3 = new PathOnS2();
        path3.CirclePath(1.8f, 0.1f);

        Layers.Add(path3);

        PathOnS2 path4 = new PathOnS2();
        path4.CirclePathVertical(1.1f, 0.1f);

        Layers.Add(path4);*/

        //PathOnS2 path2 = new PathOnS2();
        //path2.CirclePath(Mathf.PI/4, Mathf.PI / 30, Mathf.PI / 31);
        //path2.rotate = 1;
        //path2.rotation_speed = .5f;
        //Layers.Add(path2);

       // PathOnS2 path3 = new PathOnS2();
        //path3.SpiralPath(5, Mathf.PI/60);
        //path3.rotate = 1;
       // path3.rotation_speed = .8f;
        //Layers.Add(path3);
        //}

        #endregion
    }

    void Fibration()
    {
        #region Fibration
        foreach (PathOnS2 path in Layers)
        {
            List<Fibre> HopfLayer = path.Fibration();
            HopfLayers.Add(HopfLayer);
        }
        #endregion
    }

    void Projection()
    {
        #region Projection
        int layer_number = 0;
        if (HF.Count == 0)
        {
            foreach (List<Fibre> layer in HopfLayers)
            {
                List<Tube> LayerOfTubes = new List<Tube>();

                foreach (Fibre fibre in layer)
                {
                    GameObject tube = Instantiate(TubeFab, Vector3.zero, Quaternion.identity) as GameObject;
                    Tube t = tube.GetComponent<Tube>();
                    t.draw(fibre.project());
                    t.rotate = fibre.rotate;
                    t.rotation_speed = fibre.rotation_speed;
                    LayerOfTubes.Add(t);
                }
                HF.Add(LayerOfTubes);
                layer_number++;
            }
        }
        #endregion
    }

    void rotateBase(float ve)
    {
        Matrix m = new Matrix();
        m.setRotationZ((Mathf.PI / 11));
        foreach (PathOnS2 path in Layers)
        {
            for (int i = 0; i < path.Points.Count; i++)
            {
                path.Points[i].transform(m);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rotateBase(1);
        //Fibration();
        //Projection();
    }
}