using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent (typeof(Rigidbody), typeof(NavMeshAgent))]
public class EnemyMind : Mind {
	public enum State {
		Idle,
		Patrolling,
		Chasing
	}

	[SerializeField]
	private List<Transform> waypoints = new List<Transform> ();
	[SerializeField]
	private float patrolSpeed;
	[SerializeField]
	private float chaseSpeed;

	private bool isLookingAround = false;
	private bool lookingLeft = false;
	private int targetIndex;

	private State state;
	private EnemySight sight;
	private NavMeshAgent navMeshAgent;
	private new Rigidbody rigidbody;

	void Start () {
		sight = GetComponentInChildren<EnemySight> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Awake () {
		targetIndex = 0;
		SetState (State.Patrolling);
	}

	void FixedUpdate () {
		ManageBehaviour ();
	}

	protected override void Move () {
		if (currentDistance >= preferredDistance) {
			MoveTowardsTarget ();
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
		currentDistance = Vector3.Distance (transform.position, target);

		if (sight.canSeeTarget) {
			SetState (State.Chasing);
		}

		if (state == State.Chasing) {
			Chase ();
		} else if (state == State.Patrolling) {
			Patrol ();
		}

		if(isLookingAround && !sight.canSeeTarget) {
			if(lookingLeft) {
				LookLeft ();
			} else {
				LookRight ();
			}
		}
	}

	private void MoveTowardsTarget () {
		navMeshAgent.SetDestination (target);
	}

	private void Chase () {
		target = sight.lastSightingPosition;
		navMeshAgent.speed = chaseSpeed;

		if(!sight.canSeeTarget) {
			if (currentDistance <= preferredDistance) {
				StartCoroutine (LookAround ());
			}
		}

		Move ();
	}

	private void Patrol () {
		if (!waypoints.ConvertAll (waypoint => waypoint.position).Contains (target)) {
			FindNearestWaypoint ();
		}

		if (currentDistance <= preferredDistance) {
			if (targetIndex < waypoints.Count - 1) {
				targetIndex++;
			} else {
				targetIndex = 0;
			}
		}

		target = waypoints [targetIndex].position;
		navMeshAgent.speed = patrolSpeed;
		Move ();
	}

	private IEnumerator IdleWait () {
		SetState (State.Idle);
		navMeshAgent.SetDestination (transform.position);
		StartCoroutine(LookAround ());
		yield return new WaitForSeconds (2.0f);
		if (sight.canSeeTarget == false && state == State.Idle) {
			SetState (State.Patrolling);
		}
	}

	private IEnumerator LookAround() {
		isLookingAround = true;
		lookingLeft = true;
		yield return new WaitForSeconds (0.5f);
		lookingLeft = false;
		yield return new WaitForSeconds (1.0f);
		isLookingAround = false;
	}

	private void LookLeft() {
		rigidbody.MoveRotation (Quaternion.LookRotation (Vector3.Slerp (transform.forward, -transform.right, Time.deltaTime * 2f)));
	}

	private void LookRight() {
		rigidbody.MoveRotation (Quaternion.LookRotation (Vector3.Slerp (transform.forward, transform.right, Time.deltaTime * 2f)));
	}

	private void FindNearestWaypoint () {
		int nearestWaypointIndex = 0;

		for (int i = 0; i < waypoints.Count; i++) {
			if (Vector3.Distance (transform.position, waypoints [i].position) < Vector3.Distance (transform.position, waypoints [nearestWaypointIndex].position)) {
				nearestWaypointIndex = i;
			}
		}

		target = waypoints [nearestWaypointIndex].position;
	}
}
