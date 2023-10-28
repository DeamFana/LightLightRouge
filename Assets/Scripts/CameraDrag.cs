using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public Transform cameraObject;
    public float speed = 0.01f;
    public Vector3 lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            cameraObject.Translate(Vector3.right * (lastMousePosition - Input.mousePosition).x * speed);
            cameraObject.Translate(Vector3.up * (lastMousePosition - Input.mousePosition).y * speed);
        }
        lastMousePosition = Input.mousePosition;
    }
}
