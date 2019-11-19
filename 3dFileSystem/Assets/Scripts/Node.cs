using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string Path;
    public long Size;
    public float zPos;
    public bool IsDir = false;
    // public string Extension;
    // public string DateCreated;
    // public string DateModified;

    public void ProcessNode()
    {
        if (IsDir)
        {
            DirectoryInfo diTop = new DirectoryInfo(Path);

            try
            {
                float transformPositionX = 0f;
                float transformPositionY = 0f;
                float transformPositionZ = 0f;

                float initXPositon = transform.position.x;
                float initYPositon = transform.position.y;
                float initZPosition = transform.position.z + 1f;

                foreach (var fi in diTop.EnumerateFiles())
                {
                    try
                    {
                        /*if this is the first iteration, initialize the positions */

                        if (transformPositionX == 0f && transformPositionY == 0f && transformPositionZ == 0f)
                        {
                            transformPositionX = initXPositon;
                            transformPositionY = initYPositon;
                            transformPositionZ = initZPosition;
                        }
                        else if (transformPositionX < 4f)
                        {
                            transformPositionX = transformPositionX + 1f;
                        }
                        else
                        {
                            transformPositionX = 0f;
                            transformPositionY = transformPositionY - 0.99f;
                        }

                        var gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gObj.transform.position = new Vector3(transformPositionX, transformPositionY,transformPositionZ);

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
                        /*if this is the first iteration, initialize the positions */   
             

                        if (transformPositionX == 0f && transformPositionY == 0f && transformPositionZ == 0f)
                        {
                            transformPositionX = initXPositon;
                            transformPositionY = initYPositon;
                            transformPositionZ = initZPosition;
                        }
                        else if (transformPositionX < 4f)
                        {
                            transformPositionX = transformPositionX + 1f;
                        }
                        else
                        {
                            transformPositionX = 0f;
                            transformPositionY = transformPositionY - 1f;
                        }

                        var gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                        gObj.transform.position = new Vector3(transformPositionX, transformPositionY, transformPositionZ);

                        gObj.name = di.FullName;

                        gObj.AddComponent<Node>();
                        Node dn = gObj.GetComponent<Node>();
                        // dn.Size = -1;
                        // dn.Path = di.Path;
                        // dn.IsDir = true;

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
}
