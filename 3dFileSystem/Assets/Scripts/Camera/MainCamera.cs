using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //Camera mainCam;
    Camera mainCam;

    // Movement speed in units per second.
    public float smoothSpeed = 0.5f;

    public Vector3 pos1;

    public Vector3 pos2;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public bool BackButtonPressed;

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


    private void Instance_OnNodeSelected(DataNode node)
    {
        mainCam = Camera.main;
        // Keep a note of the time the movement started.


        if (BackButtonPressed)
        {
            pos1 = node.transform.position;
            pos2 = node.transform.position - new Vector3(0f, 0f, 10f);
        }
        else
        {
            //smoothSpeed = 2.0f;
            pos1 = node.transform.position;
            pos2 = node.transform.position + node.transform.forward;
            /*Debug.Log("pos1" + pos1);
            Debug.Log("pos2" + pos2);*/
        }
        // Calculate the journey length.
        journeyLength = Vector3.Distance(pos1, pos2);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        DataNode node = GridSystem.Instance.currentSelectedDataNode;
        // Debug.Log("node object" + node);

        if (node != null && node.IsDir && node.HasChild)
        {
            //create if statement to check if destination has been reached
            mainCam = Camera.main;
            
            Instance_OnNodeSelected(node);
            

            if (mainCam.transform.position == pos2)
            {
                if (BackButtonPressed)
                {
                    if (node.parentNode == null)
                    {
                        GridSystem.Instance.currentSelectedDataNode = null;

                    }
                    else if (node.IsDir && node.HasChild && node.parentNode)
                    {
                        GridSystem.Instance.currentSelectedDataNode = node.parentNode.GetComponent<DataNode>();
                    }
                    
                }
                BackButtonPressed = false;
                startTime = Time.time;
            }
            else
            {
                Instance_OnNodeSelected(node);
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * smoothSpeed;
                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;
                // Set our position as a fraction of the distance between the markers.
                mainCam.transform.position = Vector3.Lerp(transform.position, pos2, fractionOfJourney);
            }
        }

    }


}
