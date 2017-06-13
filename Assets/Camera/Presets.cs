using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presets : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Alpha1)) {
            Settings.S2List.Clear();
            Settings.S2List.Add(new S2Input("Circle path", 0.45f, Mathf.PI*2, 0.1f));

            Application.LoadLevel("Hopf scene");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {

        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {

        }
    }
}
