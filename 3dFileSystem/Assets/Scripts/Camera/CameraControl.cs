using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    // float rotationY = 0.0f;
    // float rotationX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float speed = 2.0f;
    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        //    rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        //    rotationY = Mathf.Clamp(rotationY, minY, maxY);
        //    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        //}

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }


        // if(Input.GetKey(KeyCode.UpArrow))
        // {
        //     transform.Rotate(Vector3.right, -10 * speed * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     transform.Rotate(Vector3.right, 10 * speed * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     transform.Rotate(Vector3.up, -10 * speed * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     transform.Rotate(Vector3.up, 10 * speed * Time.deltaTime);
        // }


    }
}
