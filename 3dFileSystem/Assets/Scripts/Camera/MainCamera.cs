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


    // private void Instance_OnNodeSelected(DataNode node)
    // {
    //     if (BackButtonPressed)
    //     {
    //         pos = node.transform.position - new Vector3(0f, 0f, 10f);
    //         // BackButtonPressed = false;
    //     }
    //     else
    //     {
    //         pos = node.transform.position + node.transform.forward;
    //     }
    // }

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

        if(node && BackButtonPressed)
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
                BackButtonPressed = false;
            }
        }
    }
}
