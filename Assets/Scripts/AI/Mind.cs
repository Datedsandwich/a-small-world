using UnityEngine;
using System.Collections;

public abstract class Mind : MonoBehaviour {
    [Tooltip("The location this Actor will move towards")]
    public Transform target;
    [Tooltip("Preferred distance from target, will try to maintain this.")]
    public float PreferredDistance;

    public float currentDistance;

    protected virtual void Move() {
        // Handle locomotion in this method. Needs overriding.
    }
}
