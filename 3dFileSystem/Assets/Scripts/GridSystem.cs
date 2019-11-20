using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    public Text txtSelectedDataNode;
    public Text txtHoveredOverDataNode;

    public InfoPanel infoPanel;

    public DataNode currentSelectedDataNode;

    // Start is called before the first frame update
    void Start()
    {
        txtSelectedDataNode.text = "";
        txtHoveredOverDataNode.text = "";

        infoPanel = GameObject.Find("Info Panel Gray").GetComponent<InfoPanel>();

        int i = 0;
        int colLength = 6;
        foreach (var drive in DriveInfo.GetDrives())
        {
            // Create a primitive type cube game object
            var gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Position the game object in world space
            gObj.transform.position = new Vector3(2.0f*(i%colLength), 1.0f*(i/colLength), 0.0f);
            gObj.transform.rotation = Quaternion.identity;
            gObj.name = drive.Name;

            gObj.AddComponent<DataNode>();
            DataNode dn = gObj.GetComponent<DataNode>();
            dn.Size = drive.TotalSize;
            dn.Path = drive.RootDirectory.FullName;
            dn.Name = drive.Name;
            dn.IsDir = true;
            dn.zPos = 0.0f;
            i++;
        }
    }



    RaycastHit hitInfo = new RaycastHit();
    
    void Update()
    {
        #region HANDLE MOUSE INTERACTION
        // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
        bool Hoverhit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (Hoverhit)
        {
            if (hitInfo.transform.GetComponent<DataNode>() != null)
            {
                // if there is a hit, we want to get the DataNode component to extract the information
                DataNode dn = hitInfo.transform.GetComponent<DataNode>();
                txtHoveredOverDataNode.text = $"{dn.Path}";
            }
        }
        else
        {
            txtHoveredOverDataNode.text = $"";
        }
        #endregion

        // Check to see if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if(hit)
            {
                if(hitInfo.transform.GetComponent<DataNode>()!=null)
                {
                    // if there is a hit, we want to get the DataNode component to extract the information
                    DataNode dn = hitInfo.transform.GetComponent<DataNode>();

                    // if(dn.IsFolder)
                    // {
                    //     DirectoryInfo diTop = new DirectoryInfo(dn.Path);
                    //     int samples = diTop.GetDirectories("*").Length;
                    //     dn.gameObject.transform.Translate(Vector3.forward * -(samples%2)*1.5f, Space.Self);
                    //     diTop = null;
                    // }

                    txtSelectedDataNode.text = $"Selected DataNode: {dn.Path} Size Is: {dn.Size}";
                    dn.IsSelected = true;
                    infoPanel.fillPanel(dn);
                    dn.ProcessDataNode();

                    if (currentSelectedDataNode == null)
                    {
                        currentSelectedDataNode = dn;
                    }
                    else
                    {
                        currentSelectedDataNode.IsSelected = false;
                        currentSelectedDataNode = dn;
                    }

                }
            }
        }
    }
}
