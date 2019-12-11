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

    public bool IsDirectoryEmpty(string path)
    {
        IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
        using (IEnumerator<string> en = items.GetEnumerator())
        {
            return !en.MoveNext();
        }
    }




    public void ProcessDataNode()
    {
        if (IsDir)
        {
            DirectoryInfo diTop = new DirectoryInfo(Path);
            int i = 0;
            int colLength = 6;

            try
            {
             
                if (IsDirectoryEmpty(Path))
                {
                    GameObject gObj = null;
                    gObj = Instantiate(Resources.Load("Prefabs/Astronaut")) as GameObject;
                    gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), 0, (zPos + 1f) + 10f);
                    gObj.transform.rotation = Quaternion.identity;
                    gObj.transform.SetParent(transform);
                }
                else
                {

                    foreach (var fi in diTop.EnumerateFiles())
                    {
                        try
                        {


                            var fileInfo = new System.IO.FileInfo(fi.FullName);
                           
                            GameObject gObj = null;
                            if (fi.Extension == ".txt")
                            {
                                gObj = Instantiate(Resources.Load("Prefabs/Earth")) as GameObject;
                            }

                            else if (fi.Extension == ".pdf")
                            {
                                gObj = Instantiate(Resources.Load("Prefabs/Jupiter")) as GameObject;
                            }

                            else if (fi.Extension == ".png")
                            {
                                gObj = Instantiate(Resources.Load("Prefabs/Mercury")) as GameObject;
                            }

                            else
                            {
                                gObj = Instantiate(Resources.Load("Prefabs/Pluto")) as GameObject;
                            }
                            gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), (zPos + 1f) + 10f);
                   
                            gObj.transform.rotation = Quaternion.identity;

                            gObj.name = fi.Name;
                            gObj.AddComponent<DataNode>();
                            gObj.transform.SetParent(transform);
                            DataNode dn = gObj.GetComponent<DataNode>();
                            dn.Size = fileInfo.Length;
                            dn.Path = fi.FullName;
                            dn.Name = fi.Name;
                            dn.Extension = fi.Extension;
                            dn.DateCreated = fi.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                            dn.DateModified = fi.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
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
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Debug.LogWarning($"{dirNotFound.Message}");
            }
            catch (UnauthorizedAccessException unAuthDir)
            {
                GameObject gObj = null;
                gObj = Instantiate(Resources.Load("Prefabs/Denied")) as GameObject;
                gObj.transform.position = new Vector3(transform.position.x + (2.0f * (i % colLength)), transform.position.y + (2.0f * (i / colLength)), (zPos + 1f) + 10f);
                gObj.transform.rotation = Quaternion.identity;
                gObj.transform.SetParent(transform);     
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


