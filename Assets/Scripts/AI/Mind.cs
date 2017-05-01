using UnityEngine;
using System.Collections;

public abstract class Mind : MonoBehaviour {
	[Tooltip ("The location this Actor will move towards")]
	public Vector3 target;
	[Tooltip ("Preferred distance from target, will try to maintain this.")]
	public float preferredDistance;

	public float currentDistance;

	protected virtual void Move () {
		// Handle locomotion in this method. Needs overriding.
	}
}
