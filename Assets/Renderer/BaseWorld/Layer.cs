using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour {
	void Start () {

	}

    public void draw(List<Point3D> list) {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(list.Count);
        Vector3[] arr = Point3D.toVector3List(list).ToArray();
        lineRenderer.SetWidth(.01f, .01f);
        lineRenderer.SetPositions(arr);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
