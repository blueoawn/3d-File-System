using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public bool BackButtonPressed;
    private Vector3 pos;

    private static MainCamera _instance;

    public static MainCamera Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GridSystem");
                go.AddComponent<GridSystem>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        DataNode node = GridSystem.Instance.currentSelectedDataNode;
        
        if(node && GridSystem.Instance.hitDir)
        {
            pos = node.transform.position + Vector3.forward;
            if(transform.position != pos)
            {
                transform.position = Vector3.Lerp(transform.position, pos, 0.4f);
            }
            else
            {
                GridSystem.Instance.hitDir = false;
            }
        }
        else if(node && BackButtonPressed)
        {
            if(node.parentDataNode == null)
            {
                pos = node.transform.position - Vector3.forward*10;
            }
            else
            {
                node = node.parentDataNode;
                pos = node.transform.position + Vector3.forward;
            }

            if(transform.position != pos)
            {
                transform.position = Vector3.Lerp(transform.position, pos, 0.4f);
            }
            else
            {
                if(node)
                    GridSystem.Instance.currentSelectedDataNode = node;
                BackButtonPressed = false;
            }
        }
        else
        {
            // Move Camera
            float speed = 2.0f;
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
        }
    }
}
