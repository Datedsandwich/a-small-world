using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class AcquintanceMind : Mind {
	public enum State {
		Idle,
		Patrolling,
		Chasing
	}

	public float brakingSpeed;
	public List<Transform> Waypoints = new List<Transform> ();
	public bool canSeeTarget;
	public State state;

	[SerializeField]
	private float patrolSpeed;
	[SerializeField]
	private float chaseSpeed;

	private new Rigidbody rigidbody;
	private int targetIndex;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Awake () {
		targetIndex = 0;
	}

	void FixedUpdate () {
		ManageBehaviour ();

		if (state == State.Idle) {
			return;
		}

		Quaternion rotation = CalculateRotationOnlyY ();
		// Actually turn in that direction.
		rigidbody.MoveRotation (Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * speed));
	}

	protected override void Move () {
		Vector3 tempPosition = transform.position;
		tempPosition.y = target.position.y;
		currentDistance = Vector3.Distance (tempPosition, target.position);

		if (currentDistance >= PreferredDistance) {
			shouldMove = true;
			rigidbody.velocity = MoveTowardsTarget ();
		} else {
			if (Util.RandomBool () && state != State.Idle) {
				StartCoroutine (IdleWait ());
			}
		}
	}

	public void SetState (State value) {
		state = value;
	}

	private void ManageBehaviour () {
		if (canSeeTarget) {
			SetState (State.Chasing);
		}

		if (state == State.Chasing) {
			Chase ();
		} else if (state == State.Patrolling) {
			Patrol ();
		} else {
			return;
		}
	}

	private void Patrol () {
		if (!Waypoints.Contains (target)) {
			FindNearestWaypoint ();
		}

		if (currentDistance <= PreferredDistance) {
			if (targetIndex < Waypoints.Count - 1) {
				targetIndex++;
			} else {
				targetIndex = 0;
			}

			target = Waypoints [targetIndex];
		}
		speed = patrolSpeed;
		Move ();
	}

	private Vector3 MoveTowardsTarget () {
		Vector3 velocity;

		// Check in front of us for walls
		if (!Physics.Raycast (transform.position + raycastOffset, transform.forward, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)) {
			velocity = transform.forward * speed;
			// We don't touch Y velocity to maintain integrity of gravity.
			return new Vector3 (velocity.x, rigidbody.velocity.y, velocity.z);
		}

		// Don't walk directly into walls
		return new Vector3 (0f, rigidbody.velocity.y, 0f);
	}

	private void Chase () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		speed = chaseSpeed;
		Move ();
	}

	private void Reset () {
		canSeeTarget = false;
		SetState (State.Patrolling);
	}

	private IEnumerator IdleWait () {
		SetState (State.Idle);
		yield return new WaitForSeconds (2.0f);
		if (canSeeTarget == false && state == State.Idle) {
			Reset ();
		}
	}

	private void FindNearestWaypoint () {
		// Create a temporary variable to hold the nearest waypoint index.
		int nearestWaypointIndex = 0;
		// Loop through the waypoints
		for (int i = 0; i < Waypoints.Count; i++) {
			// if the distance between the Blank and current iteration index is less than the distance between Blank and nearestWaypointIndex
			// Update the nearestWaypointIndex to the current iteration index.
			if (Vector3.Distance (transform.position, Waypoints [i].position) < Vector3.Distance (transform.position, Waypoints [nearestWaypointIndex].position)) {
				nearestWaypointIndex = i;
			}
		}
		// Set the new target.
		target = Waypoints [nearestWaypointIndex];
	}
}
