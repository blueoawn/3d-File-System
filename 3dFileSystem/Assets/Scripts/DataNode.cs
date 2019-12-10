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
    public DataNode parentDataNode;
    Camera mainCam;


    public void CollapseNode()
    {
        //transform.tranform gives me the child nodes to destroy and collapse my nodes
        //if we are in the topmost node don't collapse anything       
        if (transform.transform != null)
        {
            foreach (Transform t in transform.transform)
            {
                if(t.gameObject.name.Contains("Particle System"))
                    continue;
                Destroy(t.gameObject);
            }
        }

    }

    public void ProcessDataNode()
    {
        if (IsDir)
        {
            DirectoryInfo diTop = new DirectoryInfo(Path);

            try
            {
                int i = 0;
                int colLength = 6;
                foreach (var fi in diTop.EnumerateFiles())
                {
                    try
                    {
                        var fileInfo = new System.IO.FileInfo(fi.FullName);
                        GameObject gObj = Instantiate(Resources.Load("Prefabs/Planet")) as GameObject;

                        gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), zPos + 10f);
                        gObj.transform.rotation = Quaternion.identity;
                        gObj.name = fi.Name;
                        gObj.AddComponent<DataNode>();
                        gObj.transform.SetParent(transform);
                        DataNode dn = gObj.GetComponent<DataNode>();
                        dn.Size = fileInfo.Length;
                        dn.Path = fi.FullName;
                        dn.Name = fi.Name;
                        dn.DateCreated = fi.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
                        dn.DateModified = fi.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
                        dn.IsDir = false;
                        dn.zPos = zPos + 10f;
                        dn.parentDataNode = this;
                        HasChild = false;
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
                        GameObject gObj = Instantiate(Resources.Load("Prefabs/Galaxy")) as GameObject;

                        gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), zPos + 10f);
                        gObj.transform.rotation = Quaternion.identity;
                        gObj.name = di.Name;
                        gObj.transform.SetParent(transform);
                        gObj.AddComponent<DataNode>();
                        DataNode dn = gObj.GetComponent<DataNode>();
                        dn.Size = getFolderSize(di.FullName);
                        dn.Path = di.FullName;
                        dn.Name = di.Name;
                        dn.DateCreated = di.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
                        dn.DateModified = di.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
                        dn.IsDir = true;
                        dn.zPos = zPos + 10f;
                        dn.parentDataNode = this;
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

    public IEnumerable<FileInfo> FileInfos(string folderPath)
    {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        long folderSize;
        try
        {
            folderSize = di.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Sum(fi => fi.Length);
            Debug.Log(folderSize);
            this.Size = folderSize;
        }
        catch (UnauthorizedAccessException)
        {
            Debug.Log("UnauthorizedAccessException");
            yield break;
        }
        catch (PathTooLongException)
        {
            Debug.Log("Error path too long exception");
            yield break;
        }
        catch (System.IO.IOException)
        {
            Debug.Log("Error IOException");
            yield break;
        }
    }

    // public void GetFolderSize(IEnumerable<FileInfo> fileInfos)
    // {
    //     Debug.Log(fileInfos.ToArray().Length);
    // }

    private long getFolderSize(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        try
        {
            return di.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Sum(fi => fi.Length);
        }
        catch
        {
            return 0L;
        }
    }
}


