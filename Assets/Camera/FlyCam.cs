using UnityEngine;
using System.Collections;

public class FlyCam : MonoBehaviour
{


    public float speed = 10.0f;     // max speed of camera
    public float sensitivity = 0.25f;       // keep it from 0..1
    public bool inverted = false;


    private Vector3 lastMouse;



    // smoothing
    public bool smooth = true;
    public float acceleration = .1f;
    private float actSpeed = 0.0f;          // keep it from 0 to 1
    private Vector3 lastDir = new Vector3();
    int calls = 0;

    // Use this for initialization
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();

        lastMouse = Input.mousePosition;
        transform.position = new Vector3(0, 0, 0);

        transform.Translate(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(
                    "Mouse Down Hit the following object: " + 
                    hit.collider.name + " Index : " + hit.triangleIndex/3 + 
                    " Quaternion: " + hit.collider.gameObject.GetComponent<Tube>().Points[hit.triangleIndex / 3].quaternionVector.ToString()
                    );
                hit.collider.gameObject.GetComponent<Tube>().ColorClick((hit.triangleIndex) / 3);

                Debug.DrawRay(ray.origin, ray.direction, Color.green);
                
            }
            else
            {

                Debug.Log("Nothing was hit!");
                Debug.DrawRay(ray.origin, ray.direction, Color.green);

            }
        }
        */
        if (Input.GetKey(KeyCode.Escape)) {
            Application.LoadLevel("MainMenu");
        }
        if (calls < 400)
        {
            transform.Translate(new Vector3(0, 0, -2) * speed * Time.deltaTime);
            calls++;
            transform.LookAt(new Vector3(0, 0, 0));
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Mouse Look
            lastMouse = Input.mousePosition - lastMouse;
            if (!inverted) lastMouse.y = -lastMouse.y;
            lastMouse *= sensitivity;
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }

        lastMouse = Input.mousePosition;




        // Movement of the camera

        Vector3 dir = new Vector3();            // create (0,0,0)

        if (Input.GetKey(KeyCode.W)) dir.z += 1.0f;
        if (Input.GetKey(KeyCode.S)) dir.z -= 1.0f;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1.0f;
        if (Input.GetKey(KeyCode.D)) dir.x += 1.0f;
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
            transform.Translate(0, 0, 0);
        }
        dir.Normalize();

        if (dir != Vector3.zero)
        {
            // some movement 
            if (actSpeed < .7)
                actSpeed += acceleration * Time.deltaTime * 10;
            else
                actSpeed = 1f;

            lastDir = dir;
        }
        else
        {
            // should stop
            if (actSpeed > 0)
                actSpeed -= acceleration * Time.deltaTime * 5;
            else
                actSpeed = 0f;
        }

        if (smooth)
            transform.Translate(lastDir * actSpeed * speed * Time.deltaTime);

        else
            transform.Translate(dir * speed * Time.deltaTime);
    }

    void OnGUI()
    {
        GUILayout.Box("actSpeed: " + actSpeed.ToString());
    }
}
