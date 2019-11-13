using System;
using System.IO;
using System.Threading;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;

public class DataNode : MonoBehaviour
{
    public string Name;
    public string FullName;
    public string Extension;
    public string DateCreated;
    public string DateModified;
    public long Size;
    public bool IsFolder = false;
    public bool IsDrive = false;

    public bool IsSelected = false;
    public bool IsExpanded = false;

    public bool Move = false;
    public Vector3 NewPosition;

    //drive.AvailableFreeSpace; 
    //drive.TotalFreeSpace;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    Transform p1;
    Transform p2;
    GameObject cGObj;
    public int lengthOfLineRenderer = 2;

    // Transform parentNode;

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

    public void ProcessNode()
    {
        if(IsFolder||IsDrive)
        {
            // let's expand ...
            // Set a variable to the My Documents path.
            string docPath = FullName;

            DirectoryInfo diTop = new DirectoryInfo(docPath);

            try
            {
                int samples = diTop.GetDirectories("*").Length;
                int fileNum = diTop.GetFiles().Length;

                float rnd = 1;
                // float rndFiles = 1;

                bool randomize = true;

                if (randomize)
                {
                    rnd = UnityEngine.Random.value * samples;
                    // rndFiles = UnityEngine.Random.value * fileNum;
                }

                float offset = 2.0f / samples;
                float offsetFilres = 2.0f / fileNum;

                float increment = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
                // int i = 0;

                float radius = 4;
                float forwardSpeed = -1f;

                float xPos = 0f;
                float yPos = 0f;
                float zPos = 0f;

                Transform prevTransform = transform;
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                TimeSpan ts;
                stopWatch.Start();

                // var driveGameObj = (GameObject)Resources.Load("Prefabs/Drive", typeof(GameObject));
                //New line of code trying to create the Folder prefab game obj
                // var FolderGameObj= (GameObject)Resources.Load("Prefabs/Folder", typeof(GameObject));
                //Transform myBrick = Instantiate(driveGameObj)
                // var driveGameObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // var folderGameObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                // if (driveGameObj == null) {
                //     Debug.Log("HELLO WORLD");
                // }
                // else
                // {
                //     Debug.Log("FUCK YEAH");
                // }

                foreach (var fi in diTop.EnumerateFiles())
                {
                    try
                    {
                        Thread.Sleep(1);
                        // float transformPositionX = transform.localPosition.x + Mathf.Cos(Time.time) * radius;
                        // float transformPositionY = transform.localPosition.y + Mathf.Sin(Time.time) * radius;
                        // float transformPositionZ = transform.localPosition.z + forwardSpeed * Time.time;
                        ts = stopWatch.Elapsed;

                        xPos = Mathf.Cos(Time.time + ts.Milliseconds) * radius;
                        yPos = Mathf.Sin(Time.time + ts.Milliseconds) * radius;
                        zPos = forwardSpeed * (Time.time + ts.Milliseconds);

                        // float y = ((i * offsetFilres) - 1) + (offsetFilres / 2);
                        // float r = Mathf.Sqrt(1 - Mathf.Pow(y, 2));

                        // float phi = ((i + rnd) % fileNum) * increment;

                        // float x = Mathf.Cos(phi) * r;
                        // float z = Mathf.Sin(phi) * r;

                        var gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        gObj.transform.position = new Vector3(xPos, yPos, zPos);
                        gObj.transform.localScale *= 0.1f;

                        //gObj.transform.GetComponent<Renderer>().material.color = new Color(x, y, z);
                        //gObj.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(255, 0, 0));

                        // gObj.transform.SetParent(prevTransform);
                        gObj.name = fi.FullName;
                        gObj.transform.LookAt(prevTransform);

                        gObj.AddComponent<DataNode>();
                        DataNode dn = gObj.GetComponent<DataNode>();
                        dn.Name = fi.Name;
                        dn.Size = fi.Length;
                        dn.FullName = fi.FullName;
                        dn.Extension = fi.Extension;
                        dn.DateCreated = fi.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.DateModified = fi.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.IsFolder = false;

                        c1 = prevTransform.GetComponent<Renderer>().material.color;
                        c2 = new Color(xPos, yPos, zPos);
                        p1 = prevTransform;
                        p2 = gObj.transform;
                        // cGObj = gObj;
                        DrawConnection(p1.position, p2.position, gObj);

                        prevTransform = gObj.transform;

                        //Debug.Log($"{ fi.FullName}\t\t{fi.Parent}");

                    }
                    catch (UnauthorizedAccessException unAuthTop)
                    {
                        Debug.LogWarning($"{unAuthTop.Message}");
                    }
                    // i++;
                }

                // i = 0;
                foreach (var di in diTop.EnumerateDirectories("*"))
                {
                    try
                    {
                        Thread.Sleep(1);
                        // float transformPositionX = transform.localPosition.x + Mathf.Cos(Time.time) * radius;
                        // float transformPositionY = transform.localPosition.y + Mathf.Sin(Time.time) * radius;
                        // float transformPositionZ = transform.localPosition.z + forwardSpeed * Time.time;

                        ts = stopWatch.Elapsed;

                        Debug.Log(ts.Milliseconds);

                        xPos = Mathf.Cos(Time.time + ts.Milliseconds) * radius;
                        yPos = Mathf.Sin(Time.time + ts.Milliseconds) * radius;
                        zPos = forwardSpeed * (Time.time + ts.Milliseconds);

                        // float y = ((i * offset) - 1) + (offset / 2);
                        // float r = Mathf.Sqrt(1 - Mathf.Pow(y, 2));

                        // float phi = ((i + rnd) % samples) * increment;

                        // float x = Mathf.Cos(phi) * r;
                        // float z = Mathf.Sin(phi) * r;
               
                        // var gObj = (GameObject) Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule), new Vector3(x + transformPositionX, y + transformPositionY, z + transformPositionZ), Quaternion.identity);
                        var gObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                        gObj.transform.position = new Vector3(xPos, yPos, zPos);
                        
                        // parentNode = transform;

                        // float diScale = 0.25f;
                        //foreach (var f in di.GetFiles())
                        //    diScale += f.Length;

                        //float normalizedScale = ((diScale - 0) / (Size - 0));
                        gObj.transform.localScale *= 0.25f; //normalizedScale * Time.deltaTime;

                        //gObj.transform.GetComponent<Renderer>().material.color = new Color(x, y, z);
                        //gObj.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(255,0,0));

                        // gObj.transform.SetParent(prevTransform);
                        gObj.name = di.FullName;
                        gObj.transform.LookAt(prevTransform);
                        gObj.transform.Translate(Vector3.forward * -(samples%2), Space.Self);

                        gObj.AddComponent<DataNode>();
                        DataNode dn = gObj.GetComponent<DataNode>();
                        dn.Name = di.Name;
                        //dn.Size = GetDirectorySize(di.FullName);
                        dn.Size = -1;
                        dn.FullName = di.FullName;
                        dn.IsFolder = true;
                        dn.DateCreated = di.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        dn.DateModified = di.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm tt");

                        c1 = transform.GetComponent<Renderer>().material.color;
                        c2 = new Color(xPos, yPos, zPos);
                        p1 = prevTransform;
                        p2 = gObj.transform;
                        DrawConnection(p1.position, p2.transform.position, gObj);
                        
                        prevTransform = gObj.transform;

                        Debug.Log($"{ di.FullName}\t\t{di.Parent}");

                    }
                    catch (UnauthorizedAccessException unAuthDir)
                    {
                        Debug.LogWarning($"{unAuthDir.Message}");
                    }
                    // i++;
                }

                stopWatch.Stop();
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

    // int ProcessFiles(DirectoryInfo diTop, int i)
    // {
    //     int samples = diTop.GetDirectories("*").Length;
    //     float rnd = 1;
    //     bool randomize = true;

    //     if (randomize)
    //         rnd = UnityEngine.Random.value * samples;

    //     float offset = 2.0f / samples;
    //     float increment = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));

    //     foreach (var fi in diTop.EnumerateFiles())
    //     {
    //         try
    //         {
    //             float y = ((i * offset) - 1) + (offset / 2);
    //             float r = Mathf.Sqrt(1 - Mathf.Pow(y, 2));

    //             float phi = ((i + rnd) % samples) * increment;

    //             float x = Mathf.Cos(phi) * r;
    //             float z = Mathf.Sin(phi) * r;

    //             var gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //             gObj.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, z + transform.position.z);
    //             gObj.transform.localScale *= 0.1f;

    //             gObj.transform.GetComponent<Renderer>().material.color = new Color(x, y, z);
    //             gObj.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(255, 0, 0));

    //             gObj.transform.SetParent(transform);
    //             gObj.name = fi.FullName;
    //             gObj.transform.LookAt(transform);

    //             gObj.AddComponent<DataNode>();
    //             DataNode dn = gObj.GetComponent<DataNode>();
    //             dn.Name = fi.Name;
    //             dn.Size = -1;
    //             dn.FullName = fi.FullName;
    //             dn.IsFolder = false;

    //             c1 = transform.GetComponent<Renderer>().material.color;
    //             c2 = new Color(x, y, z);
    //             p1 = transform;
    //             p2 = gObj.transform;
    //             cGObj = gObj;
    //             DrawConnection(p1.position, p2.position, cGObj);


    //             //Debug.Log($"{ fi.FullName}\t\t{fi.Parent}");
    //         }
    //         catch (UnauthorizedAccessException unAuthTop)
    //         {
    //             Debug.LogWarning($"{unAuthTop.Message}");
    //         }
    //         i++;
    //     }
    //     return i;
    // }

    void DrawConnection(Vector3 p1, Vector3 p2, GameObject go)
    {
        LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.positionCount = lengthOfLineRenderer;


        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        lineRenderer.SetPosition(0, p1);
        lineRenderer.SetPosition(1, p2);
    }
    public long GetFolderSize(string folderPath)
    {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        return di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
    }
    void Update()
    {
        //if (IsSelected)
        //{
        //    transform.Rotate(Vector3.up, 25 * Time.deltaTime);
        //}

        // if (Move)
        // {
        //     //// Distance moved equals elapsed time times speed..
        //     //float distCovered = (Time.time - startTime) * speed;

        //     //// Fraction of journey completed equals current distance divided by total distance.
        //     //float fractionOfJourney = distCovered / journeyLength;

        //     // Set our position as a fraction of the distance between the markers.
        //     transform.localPosition = Vector3.Lerp(transform.localPosition, NewPosition, Time.deltaTime); ;
        //     GetComponent<LineRenderer>().SetPosition(1, transform.position);
        //     if (transform.localPosition.Equals(NewPosition))
        //     {
        //         Move = false;
        //     }
        // }
    }

}
