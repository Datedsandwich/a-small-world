using UnityEngine;
using System.Collections;

public abstract class Mind : MonoBehaviour {
    [Tooltip("Enables Debug.DrawRay")]
    public bool Debugging;
    [Tooltip("Movement Speed")]
    public float speed;
    [Tooltip("The location this Actor will move towards")]
    public Transform target;
    [Tooltip("The layer of objects that Actor will avoid")]
    public LayerMask layerMask;
    [Tooltip("How far Actor will stay away from obstacles")]
    public float ObstacleDistance;
    [Tooltip("Should the Actor move towards it's target?")]
    public bool shouldMove;
    [Tooltip("Preferred distance from target, will try to maintain this.")]
    public float PreferredDistance;
    [Tooltip("Minimum distance from target, won't get closer than this.")]
    public float MinDistance;
    [Tooltip("Offset for Raycast origin. Rays will be cast from transform.position + raycastOffset")]
    public Vector3 raycastOffset;

    protected float currentDistance;

    protected virtual Quaternion CalculateRotation() {
        return Quaternion.LookRotation(CalculateLookAtDirection((target.position - transform.position).normalized));
    }

    protected virtual Quaternion CalculateRotationOnlyY() {
        // Store a temp position so that the actor will only take into account horizontal distance between itself and it's target
        Vector3 tempPosition = target.position;
        tempPosition.y = transform.position.y;
        Vector3 direction = CalculateLookAtDirection((tempPosition - transform.position).normalized);

        return Quaternion.LookRotation(direction);
    }

    protected virtual Vector3 CalculateLookAtDirection(Vector3 direction) {
        if (shouldMove) {
            RaycastHit hit;
            Vector3 leftRay = transform.forward - (transform.right);
            Vector3 rightRay = transform.forward + (transform.right);

            if (Physics.Raycast(transform.position + raycastOffset, leftRay, out hit, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)
                && Physics.Raycast(transform.position + raycastOffset, rightRay, out hit, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)) {
                if (hit.transform != transform) {
                    return direction;
                }
            }

            if (Debugging) {
                Debug.DrawRay(transform.position + raycastOffset, transform.forward * ObstacleDistance, Color.yellow);
                Debug.DrawRay(transform.position + raycastOffset, leftRay * ObstacleDistance, Color.red);
                Debug.DrawRay(transform.position + raycastOffset, rightRay * ObstacleDistance, Color.red);
            }

            // Check for forward raycast.
            if (Physics.Raycast(transform.position + raycastOffset, transform.forward, out hit, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)) {
                if (hit.transform != transform) {
                    direction += hit.normal * 50;
                }
            }

            // Check for left and right raycast.
            if (Physics.Raycast(transform.position + raycastOffset, leftRay, out hit, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)
                || Physics.Raycast(transform.position + raycastOffset, rightRay, out hit, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)) {
                if (hit.transform != transform) {
                    direction += hit.normal * ObstacleDistance;
                }
            }
        }
        return direction;
    }

    protected virtual void Move() {
        // Handle locomotion in this method. Needs overriding.
    }
}
