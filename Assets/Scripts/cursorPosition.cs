using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorPosition : MonoBehaviour
{
    Camera cam;

    LayerMask mask = 1 << 8;

    public Transform cursorPositionn;

    public LineRenderer line;

    public Transform temp;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            cursorPositionn.transform.position = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            speed = 600;
        }

        if (Input.GetMouseButtonDown(1))
        {
            speed = -500;
        }

        cursorPositionn.transform.rotation = Quaternion.FromToRotation(-transform.forward, hit.normal) * transform.rotation;
        transform.Rotate(0, 0, speed * Time.deltaTime);

        if (speed > 70)
        {
            speed -= 10;
        }

        if (speed < 70)
        {
            speed += 10;
        }
    }
}
