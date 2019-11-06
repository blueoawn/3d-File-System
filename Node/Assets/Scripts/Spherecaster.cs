using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spherecaster : MonoBehaviour {
		
		public GameObject currentHitObject;
		private float currentHitDistance;
		public float sphereRadius;
		private Vector3 origin;
		private Vector3 direction;
		public float maxDistance;
		public LayerMask layerMask;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		origin = transform.position;
		direction = transform.forward;
		RaycastHit hit;
		if(Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal)){
			currentHitObject = hit.transform.gameObject;

		}
		else{
			currentHitDistance = maxDistance;
			currentHitObject = null;
		}
	}
	public GameObject getCurrentHitObject() {
		return currentHitObject;
	}

	private void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Debug.DrawLine(origin, origin + direction * currentHitDistance);
		Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
	}
}
