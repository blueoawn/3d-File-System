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

    private float normalize(long fileSize, float max)
    {
        if((float)fileSize > max)
            fileSize = (long)max;

        float norm_val = 2f + ((fileSize - 0f)*(4f - 2f))/(max - 0f);
        
        return norm_val;
    }

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
                        float normVal = normalize(fileInfo.Length, 1e7f);
                        var gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gObj.transform.position = new Vector3(transform.position.x + (4.5f * (i % colLength)), transform.position.y + (4.5f * (i / colLength)), (zPos + 1f) + 10f);
                        gObj.transform.rotation = Quaternion.identity;
                        gObj.transform.localScale = new Vector3(normVal, normVal, 1f);
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
                        long folderSize = GetFolderSize(di.FullName);
                        float normVal = normalize(folderSize, 1e10f);
                        var gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        gObj.transform.position = new Vector3(transform.position.x + (4.5f * (i % colLength)), transform.position.y + (4.5f * (i / colLength)), (zPos + 1f) + 10f);
                        gObj.transform.rotation = Quaternion.identity;
                        gObj.transform.localScale = new Vector3(normVal, normVal, 1f);
                        gObj.name = di.Name;
                        gObj.transform.SetParent(transform);
                        gObj.AddComponent<DataNode>();
                        DataNode dn = gObj.GetComponent<DataNode>();
                        dn.Size = folderSize;
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
        long size = 0L;
        try
        {
            size = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }
        catch(UnauthorizedAccessException unAuthDir)
        {
            Debug.LogWarning($"{unAuthDir.Message}");
        }
        
        return size;
    }

    // static long GetDirectorySize(string p)
    // {
    //     string[] a = Directory.GetFiles(p, "*.*");

    //     long b = 0;
    //     foreach (string name in a)
    //     {
    //         FileInfo info = new FileInfo(name);
    //         b += info.Length;
    //     }
        
    //     return b;
    // }
}


