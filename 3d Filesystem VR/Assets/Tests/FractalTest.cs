using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTest : MonoBehaviour
{
	public Material material;
	public int maxDepth;
	public float childScale = 0.5f;

	private int depth = 0;
	private Material[,] _materials;

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
	public float maxRotationSpeed;
	public float maxTwist;

	private float rotationSpeed;

	// Use this for initialization
	void Start ()
	{
		rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
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

	public float spawnProbability;

	private IEnumerator CreateChildren()
	{
		for (int i = 0; i < childDirections.Length; i++)
		{
			if (Random.value < spawnProbability)
			{
				yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
               	new GameObject("Fractal Child").AddComponent<FractalTest>().
                	Initialize(this, i);
			}
			
		}
	}

	/*private IEnumerator CreatChildren()
	{
		yield return new WaitForSeconds(0.5f);
		new GameObject("Fractal Child").AddComponent<Fractal>().
			Initialize(this, Vector3.up, Quaternion.identity);
		yield return new WaitForSeconds(0.5f);
		new GameObject("Fractal Child").AddComponent<Fractal>().
			Initialize(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
		yield return new WaitForSeconds(0.5f);
		new GameObject("Fractal Child").AddComponent<Fractal>().
			Initialize(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
	}*/

	private void Initialize(FractalTest parent, int childIndex)
	{
		meshes = parent.meshes;
		_materials = parent._materials;
		material = parent.material;
		maxDepth = parent.maxDepth;
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = 
			childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
