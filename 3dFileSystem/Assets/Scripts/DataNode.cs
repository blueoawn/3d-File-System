using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataNode : MonoBehaviour
{
    public string Name;
    public string Path;
    public long Size;
    public float zPos;
    public bool IsDir = false;
    public string Extension;
    public string DateCreated;
    public string DateModified;
    public bool HasChild = false;
    public bool IsSelected = false;
    public bool IsExpanded = false;
    public Transform parentNode;
    DataNode parentDataNode;
    Camera mainCam;


    public void CollapseNode()
    {
        //transform.tranform gives me the child nodes to destroy and collapse my nodes
        //if we are in the topmost node don't collapse anything       
        if (transform.transform != null)
        {
            foreach (Transform t in transform.transform)
            {
                Destroy(t.gameObject);
            }
        }

    }




    public void ProcessDataNode()
    {
        if (IsDir)
        {
            DirectoryInfo diTop = new DirectoryInfo(Path);


            //parentDataNode = 
            try
            {
                // float transformPositionX = 0f;
                // float transformPositionY = 0f;
                // float transformPositionZ = 0f;

                // float initXPositon = transform.position.x;
                // float initYPositon = transform.position.y;
                // float initZPosition = transform.position.z + 1f;
                int i = 0;
                int colLength = 6;
                foreach (var fi in diTop.EnumerateFiles())
                {
                    try
                    {
                        var fileInfo = new System.IO.FileInfo(fi.FullName);
                        //var gObj = (GameObject)Resources.Load("Prefabs/Planet", typeof(GameObject));
                        GameObject gObj = Instantiate(Resources.Load("Prefabs/Planet")) as GameObject;

                        gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), (zPos + 1f) + 10f);
                        //var gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), (zPos + 1f) + 10f);
                        gObj.transform.rotation = Quaternion.identity;
                        gObj.name = fi.Name;
                        gObj.AddComponent<DataNode>();
                        gObj.transform.SetParent(transform);
                        DataNode dn = gObj.GetComponent<DataNode>();
                        dn.Size = fileInfo.Length;
                        dn.Path = fi.FullName;
                        dn.Name = fi.Name;
                        dn.DateCreated = fi.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.DateModified = fi.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.IsDir = false;
                        dn.zPos = (zPos + 1f) + 10f;
                        dn.parentNode = transform;
                        HasChild = true;
                        i++;
                    }
                    catch (UnauthorizedAccessException unAuthTop)
                    {
                        Debug.LogWarning($"{unAuthTop.Message}");
                    }
                }

                foreach (var di in diTop.EnumerateDirectories("*"))
                {
                    try
                    {
                        System.IO.DirectoryInfo dirinfo = new DirectoryInfo(di.FullName);
                        //var gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        //gObj.transform.position = new Vector3(2.0f * (i % colLength), 2.0f * (i / colLength), (zPos + 1f) + 10f);
                        GameObject gObj = Instantiate(Resources.Load("Prefabs/Galaxy")) as GameObject;

                        gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), (zPos + 1f) + 10f);
                        gObj.transform.rotation = Quaternion.identity;
                        gObj.name = di.Name;
                        gObj.transform.SetParent(transform);
                        gObj.AddComponent<DataNode>();
                        DataNode dn = gObj.GetComponent<DataNode>();
                        //dn.Size = GetFolderSize(di.FullName);
                        dn.Path = di.FullName;
                        dn.Name = di.Name;
                        dn.DateCreated = di.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.DateModified = di.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.IsDir = true;
                        dn.zPos = (zPos + 1f) + 10f;
                        dn.parentNode = transform;
                        HasChild = true;
                        i++;
                    }
                    catch (UnauthorizedAccessException unAuthDir)
                    {
                        Debug.LogWarning($"{unAuthDir.Message}");
                    }
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Debug.LogWarning($"{dirNotFound.Message}");
            }
            catch (UnauthorizedAccessException unAuthDir)
            {
                Debug.LogWarning($"unAuthDir: {unAuthDir.Message}");
            }
            catch (PathTooLongException longPath)
            {
                Debug.LogWarning($"{longPath.Message}");
            }
        }
    }

    public long GetFolderSize(string folderPath)
    {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        return 0;
        //return di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
    }
}


