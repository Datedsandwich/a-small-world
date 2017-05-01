using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AcquintanceMind), typeof(SphereCollider))]
public class AcquintanceSight : MonoBehaviour {
	public LayerMask layerMask;
	private Transform head;

	[SerializeField]
	private float fieldOfViewAngle = 45f;
	private bool checkingLOS;

	private AcquintanceMind mind;

	void Start () {
		mind = GetComponent<AcquintanceMind> ();
		head = transform.FindChild ("Head");
	}
	
	void OnTriggerStay(Collider other) {
		if(other.CompareTag("Player")) {
			float angle = Vector3.Angle (head.forward, other.transform.position - transform.position);

			if(angle < fieldOfViewAngle) {
				if(!Physics.Linecast(head.position, other.transform.position, layerMask)) {
					mind.canSeeTarget = true;
				}
			} else {
				mind.canSeeTarget = false;
			}
		}
	}
}
