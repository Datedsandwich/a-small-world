using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AcquintanceMind), typeof(SphereCollider))]
public class AcquintanceSight : MonoBehaviour {
	[SerializeField]
	private float fieldOfViewAngle = 45f;
	private bool checkingLOS;

	private AcquintanceMind mind;

	void Start () {
		mind = GetComponent<AcquintanceMind> ();
	}
	
	void OnTriggerStay(Collider other) {
		if(other.CompareTag("Player")) {
			float angle = Vector3.Angle (transform.forward, other.transform.position - transform.position);

			if(angle < fieldOfViewAngle) {
				if(!Physics.Linecast(transform.position, other.transform.position, mind.layerMask)) {
					mind.canSeeTarget = true;
				}
			} else {
				mind.canSeeTarget = false;
			}
		}
	}
}
