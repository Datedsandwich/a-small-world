using UnityEngine;
using System.Collections;

public abstract class Mind : MonoBehaviour {
	[Tooltip ("The location this Actor will move towards")]
	protected Vector3 target;
	[Tooltip ("Preferred distance from target, will try to maintain this.")]
	public float preferredDistance;

	protected float currentDistance;

	protected virtual void Move () {
		// Handle locomotion in this method. Needs overriding.
	}
}
