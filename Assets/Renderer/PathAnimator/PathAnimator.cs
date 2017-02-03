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
        noOfLayers = 3;

        main();
        //transform.Translate(0, 0, Time.deltaTime * 1);
    }

    void main()
    {
        Base();
        Fibration();
        Projection();
    }

    void Base()
    {
        #region Base
        for (int i = 0; i < noOfLayers; i++)
        {
            PathOnS2 path = new PathOnS2();
            path.setPath(noOfLayers, i);
            Layers.Add(path);
        }

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
                    LayerOfTubes.Add(t);
                }
                HF.Add(LayerOfTubes);
            }
        }
            /*
        else
        {
            for (int i = 0; i < HF.Count; i++)
            {
                //every layer in hf

                for (int j = 0; j < HF[i].Count; j++)
                {
                    //every fibre in hf
                    HF[i][j].clear();
                    HF[i][j].draw(HopfLayers[i][j].project());
                    //Debug.Log("debug 1");
                }
            }

            int j = 0;
            foreach (List<Fibre> layer in HopfLayers)
            {
                int i = 0;
                foreach (Fibre fibre in layer)
                {
                    //Destroy(HF[j][i].gameObject);
                    HF[j][i].clear();
                    HF[j][i].draw(fibre.project());
                    i++;
                }
                j++;
            }

        }*/
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
