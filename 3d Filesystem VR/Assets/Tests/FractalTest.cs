﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTest : MonoBehaviour
{
	public Material material;
	public int maxDepth;
	public float childScale = 0.5f;

	private int depth = 0;
	private Material[,] _materials;
	private GameObject camera;

	private void InitializeMaterials()
	{
		_materials = new Material[maxDepth + 1, 2];
		for (int i = 0; i <= maxDepth; i++)
		{
			float t = i / (maxDepth - 1f);
			t *= t;
			_materials[i, 0] = new Material(material);
			_materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
			_materials[i, 1] = new Material(material);
			_materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
		}

		_materials[maxDepth, 0].color = Color.magenta;
		_materials[maxDepth, 1].color = Color.red;
	}

	public Mesh[] meshes;

	// Use this for initialization
	void Start ()
	{
		// camera = GameObject.Find("Main Camera");
		// camera.transform.localPosition = transform.localPosition;
		// camera.transform.LookAt(transform);

		if (_materials == null)
		{
			InitializeMaterials();
		}
		gameObject.AddComponent<MeshFilter>().mesh = 
			meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer>().material = 
			_materials[depth, Random.Range(0, 2)];
		if (depth < maxDepth)
		{
			StartCoroutine(CreateChildren());
		}
	}

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward, 
		Vector3.back
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f) 
	};

	private IEnumerator CreateChildren()
	{
		yield return new WaitForSeconds(0.25f);
		new GameObject("Fractal Child").AddComponent<FractalTest>().
			Initialize(this);
	}

	public float forwardSpeed = -1f; // Assuming negative Z is towards the camera
	public float radius = 4;

	private void Initialize(FractalTest parent)
	{
		float xPos = parent.transform.localPosition.x + Mathf.Cos(Time.time) * radius;
		float yPos = parent.transform.localPosition.y + Mathf.Sin(Time.time) * radius;
		float zPos = parent.transform.localPosition.z + forwardSpeed * Time.time;
		meshes = parent.meshes;
		_materials = parent._materials;
		material = parent.material;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		Vector3 vec = new Vector3(xPos, yPos, zPos);
		transform.localPosition = vec * (0.5f + 0.5f * childScale);
		transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
	}

	// Update is called once per frame
	void Update ()
	{
		// transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
