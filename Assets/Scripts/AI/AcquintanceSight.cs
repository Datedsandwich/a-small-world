using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AcquintanceMind), typeof(SphereCollider))]
public class AcquintanceSight : MonoBehaviour {
	public LayerMask layerMask;

	[SerializeField]
	private float fieldOfViewAngle = 110f;
	private AcquintanceMind mind;
	private Transform head;

	void Start () {
		mind = GetComponent<AcquintanceMind> ();
		head = transform.FindChild ("Head");
	}

	void OnTriggerStay (Collider other) {
		if (other.CompareTag (Tags.player)) {
			float angle = Vector3.Angle (head.forward, other.transform.position - transform.position);

			if (angle < fieldOfViewAngle) {
				if (!Physics.Linecast (head.position, other.transform.position, layerMask)) {
					mind.canSeeTarget = true;
					mind.lastSightingPosition = other.transform.position;
				}
			} else {
				mind.canSeeTarget = false;
			}
		}
	}
}
