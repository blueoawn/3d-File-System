using UnityEngine;
using System.Collections;

public class ScrollUV : MonoBehaviour {

	void Update () {

		MeshRenderer mr = GetComponent<MeshRenderer>();

		Material mat = mr.material;

		Vector2 offset = mat.mainTextureOffset;

		offset.x += Time.deltaTime / 100f;

		mat.mainTextureOffset = offset;

	}

}
