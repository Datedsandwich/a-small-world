using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EnemyMind), typeof(SphereCollider))]
public class EnemySight : MonoBehaviour {
	public LayerMask layerMask;

	[SerializeField]
	private float fieldOfViewAngle = 110f;
	private EnemyMind mind;
	private Transform head;

	void Start () {
		mind = GetComponent<EnemyMind> ();
		head = transform.FindChild ("Head");
	}

	void OnTriggerStay (Collider other) {
		if (other.CompareTag (Tags.player)) {
			float angle = Vector3.Angle (head.forward, other.transform.position - transform.position);

			if (angle <= fieldOfViewAngle) {
				if (!Physics.Linecast (head.position, other.transform.position, layerMask)) {
					CanSeeTarget ();
					mind.lastSightingPosition = other.transform.position;
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
			mind.canSeeTarget = false;
		}
	}

	private void CanSeeTarget() {
		mind.canSeeTarget = true;
	}

	private void CannotSeeTarget() {
		mind.canSeeTarget = false;
	}
}
