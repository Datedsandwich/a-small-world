using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent (typeof(Rigidbody), typeof(NavMeshAgent))]
public class AcquintanceMind : Mind {
	public enum State {
		Idle,
		Patrolling,
		Chasing
	}

	public bool canSeeTarget;

	[SerializeField]
	private List<Transform> waypoints = new List<Transform> ();
	public State state;

	[SerializeField]
	private float patrolSpeed;
	[SerializeField]
	private float chaseSpeed;
	private int targetIndex;

	private NavMeshAgent navMeshAgent;
	private Transform player;


	void Start () {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Awake () {
		targetIndex = 0;
		SetState (State.Patrolling);
	}

	void FixedUpdate () {
		ManageBehaviour ();
	}

	protected override void Move () {
		if (currentDistance >= PreferredDistance) {
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
		currentDistance = Vector3.Distance (transform.position, target.position);

		if (canSeeTarget) {
			SetState (State.Chasing);
		}

		if (state == State.Chasing) {
			Chase ();
		} else if (state == State.Patrolling) {
			Patrol ();
		}
	}

	private void MoveTowardsTarget () {
		navMeshAgent.SetDestination (target.position);
	}

	private void Chase () {
		target = player;
		navMeshAgent.speed = chaseSpeed;

		if (!canSeeTarget) {
			StartCoroutine (IdleWait ());
		}

		Move ();
	}

	private void Patrol () {
		if (!waypoints.ConvertAll (waypoint => waypoint.position).Contains (target.position)) {
			FindNearestWaypoint ();
		}

		if (currentDistance <= PreferredDistance) {
			if (targetIndex < waypoints.Count - 1) {
				targetIndex++;
			} else {
				targetIndex = 0;
			}
		}

		target = waypoints [targetIndex];
		navMeshAgent.speed = patrolSpeed;
		Move ();
	}

	private IEnumerator IdleWait () {
		SetState (State.Idle);
		navMeshAgent.SetDestination (transform.position);
		yield return new WaitForSeconds (2.0f);
		if (canSeeTarget == false && state == State.Idle) {
			SetState (State.Patrolling);
		}
	}

	private void FindNearestWaypoint () {
		Debug.Log ("Finding nearest waypoint");
		int nearestWaypointIndex = 0;

		for (int i = 0; i < waypoints.Count; i++) {
			if (Vector3.Distance (transform.position, waypoints [i].position) < Vector3.Distance (transform.position, waypoints [nearestWaypointIndex].position)) {
				nearestWaypointIndex = i;
			}
		}

		target = waypoints [nearestWaypointIndex];
	}
}
