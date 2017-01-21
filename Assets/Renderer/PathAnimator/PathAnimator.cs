using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PathAnimator : MonoBehaviour {

    public GameObject TubeFab;
    //public LineRenderer Renderer;

    List<PathOnS2> Layers;
    List<List<Fibre>> HopfLayers;
    List<List<Tube>> HF;
    int noOfLayers;

	// Use this for initialization
	void Start () {
        Layers = new List<PathOnS2>();
        HopfLayers = new List<List<Fibre>>();
        HF = new List<List<Tube>>();
        noOfLayers = 3;

        //transform.Translate(0, 0, Time.deltaTime * 1);

        #region Base
        for (int i = 0; i < noOfLayers; i++)
        {
            PathOnS2 path = new PathOnS2();
            path.setPath(noOfLayers, i);
            Layers.Add(path);
        }

        #endregion

        #region Fibration
        foreach (PathOnS2 path in Layers)
        {
            List<Fibre> HopfLayer = path.Fibration();
            HopfLayers.Add(HopfLayer);
        }
        #endregion

        #region Projection
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
        #endregion


    }

    // Update is called once per frame
    void Update () {

    }
}
