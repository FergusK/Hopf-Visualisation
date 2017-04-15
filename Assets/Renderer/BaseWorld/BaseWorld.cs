using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorld : MonoBehaviour {

    List<S2Input> list;
    public GameObject LayerObject;

	// Use this for initialization
	void Start () {

        list = Settings.S2List;

        foreach (S2Input s2layer in Settings.S2List)
        {
            Debug.Log("exists!");
            PathOnS2 path = new PathOnS2();

            if (s2layer.type.Equals("Circle path"))
            {
                Debug.Log("inside!");
                path.CirclePath(s2layer.FloatParam1, s2layer.FloatParam2, s2layer.FloatParam3);
            }
            else if (s2layer.type.Equals("Circle path vertical"))
            {
                path.CirclePathVertical(s2layer.FloatParam1, s2layer.FloatParam2, s2layer.FloatParam3);
            }
            else if (s2layer.type.Equals("Spiral path"))
            {
                path.SpiralPath(s2layer.IntParam1);
            }
            else if (s2layer.type.Equals("Spiral horizontal path"))
            {
                path.SpiralPathHorizontal(s2layer.IntParam1);
            }

            path.rotate = s2layer.rotate;
            if (s2layer.rotate == 1 || s2layer.rotate == 2)
                path.rotation_speed = s2layer.rotation_speed;

            //Layers.Add(path);
            List<Point3D> points = new List<Point3D>();
            foreach (Point3D point in path.Points) {
                Point3D newPoint = new Point3D(point.x, point.z, point.y);
                newPoint.Latitude = point.Latitude;
                newPoint.Longitude = point.Longitude;
                points.Add(newPoint);
            }

            print("SIZE OF THE LIST : " + points.Count);
            
            GameObject layer = Instantiate(LayerObject, Vector3.zero, Quaternion.identity) as GameObject;
            Layer newLayer = layer.GetComponent<Layer>();

            newLayer.draw(points);

        }

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.LoadLevel("MainMenu");
        }
	}
}
