using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class EnemySight : MonoBehaviour {
	public LayerMask layerMask;
	public bool canSeeTarget;
	public Vector3 lastSightingPosition;


	[SerializeField]
	private float fieldOfViewAngle = 110f;

	void OnTriggerStay (Collider other) {
		if (other.CompareTag (Tags.player)) {
			float angle = Vector3.Angle (transform.forward, other.transform.position - transform.position);

			if (angle <= fieldOfViewAngle) {
				if (!Physics.Linecast (transform.position, other.transform.position, layerMask)) {
					CanSeeTarget ();
					lastSightingPosition = other.transform.position;
				} else {
					CannotSeeTarget ();
				}
			} else {
				CannotSeeTarget ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.CompareTag(Tags.player)) {
			CannotSeeTarget ();
		}
	}

	private void CanSeeTarget() {
		canSeeTarget = true;
	}

	private void CannotSeeTarget() {
		canSeeTarget = false;
	}
}
