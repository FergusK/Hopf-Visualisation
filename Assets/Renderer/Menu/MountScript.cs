using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MountScript : MonoBehaviour
{
    public Transform currentMount;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentMount.position, 0.05f);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, 0.05f);
    }

    public void setMount(Transform newMount)
    {
        currentMount = newMount;
    }

    public void Quit() {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
