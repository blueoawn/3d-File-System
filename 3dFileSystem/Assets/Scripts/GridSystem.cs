using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GridSystem : MonoBehaviour
{

	// public delegate void NodeSelected(DataNode node);
	// public event NodeSelected OnNodeSelected;

	public Text txtSelectedDataNode;
	public Text txtHoveredOverDataNode;

	public InfoPanel infoPanel;
	Camera mainCam;
	public DataNode currentSelectedDataNode;
	public float smoothSpeed = 0.0125f;
	GameObject textGameObject;
	public bool hitDir;

	private static GridSystem _instance;

	//we are creating a static object so we can preserve the data going forward
	public static GridSystem Instance
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


	//awake is called before start and before the first frame update
	void Awake()
	{
		_instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{

		infoPanel = GameObject.Find("Info Panel Gray").GetComponent<InfoPanel>();

		int i = 0;
		int colLength = 6;
		foreach (var drive in DriveInfo.GetDrives())
		{
			if(isDirEmpty(drive.RootDirectory.FullName))
				continue;

			 GameObject gObj = Instantiate(Resources.Load("Prefabs/Galaxy")) as GameObject;

			// Position the game object in world space
			gObj.transform.position = new Vector3(2.0f * (i % colLength), 1.0f * (i / colLength), 0.0f);
			gObj.transform.rotation = Quaternion.identity;
			gObj.name = drive.Name;

			gObj.AddComponent<DataNode>();
			DataNode dn = gObj.GetComponent<DataNode>();
			dn.Size = drive.TotalSize;
			dn.Path = drive.RootDirectory.FullName;
			dn.Name = drive.Name;
			dn.DateCreated = drive.RootDirectory.CreationTime.ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
            dn.DateModified = drive.RootDirectory.LastWriteTime.ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
			dn.IsDir = true;
			dn.HasChild = true;
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

				if (textGameObject == null)
				{
					textGameObject = new GameObject("text");
				}
				else
				{
					Destroy(textGameObject);
					textGameObject = new GameObject("text");
				}

				textGameObject.transform.parent = dn.transform;

				TextMeshPro myText = textGameObject.AddComponent<TextMeshPro>();
				myText.text = $"{dn.Path}";
				myText.fontSize = 4;
				RectTransform rt = myText.GetComponent<RectTransform>();
				//making all the margins zero and so that the text appears where we want it to
				rt.position = dn.transform.position;
				rt.anchorMax = Vector3.zero;
				rt.anchorMin = Vector3.zero;
				rt.pivot = Vector3.zero;
				rt.sizeDelta = new Vector2(5f, 1.5f);
			}
			else
			{
				if (textGameObject != null)
				{
					Destroy(textGameObject);
				}
			}
		}
		#endregion

		// Check to see if the Left Mouse Button was clicked
		if (Input.GetMouseButtonDown(0))
		{
			// Create a raycase from the screen-space into World Space, store the data in hitInfo Object
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{
				if (hitInfo.transform.GetComponent<DataNode>() != null)
				{
					// if there is a hit, we want to get the DataNode component to extract the information
					DataNode dn = hitInfo.transform.GetComponent<DataNode>();

					dn.ProcessDataNode();
					infoPanel.fillPanel(dn);
					if(dn.IsDir)
						hitDir = true;
					else
						hitDir = false;
					currentSelectedDataNode = dn;
					//do camera movement functionality

					//if my selected node is a directory and it has children

					// _instance.currentSelectedDataNode = currentSelectedDataNode;

					// if (OnNodeSelected != null)
					// {
					// 	Debug.Log("Hello World");
					// 	OnNodeSelected(dn);
					// }
				}
			}
		}
	}

	private bool isDirEmpty(string folderPath)
    {
		DirectoryInfo di = new DirectoryInfo(folderPath);
		long size;
		try
		{
			size = di.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Sum(fi => fi.Length);
		}
		catch
		{
			size = 0L;
		}
		return size == 0L;
    }
}


