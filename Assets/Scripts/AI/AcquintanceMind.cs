using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AcquintanceMind : Mind {
    public float brakingSpeed;

    private new Rigidbody rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        Quaternion rotation = CalculateRotationOnlyY();
        // Actually turn in that direction.
        rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed));
        Move();
    }

    protected override void Move() {
        Vector3 tempPosition = transform.position;
        tempPosition.y = target.position.y;
        currentDistance = Vector3.Distance(tempPosition, target.position);

        if (currentDistance >= PreferredDistance) {
            shouldMove = true;
            rigidbody.velocity = GiveChase();
        } else if (currentDistance > MinDistance) {
            rigidbody.velocity = MaintainDistance();
            shouldMove = false;
        } else {
            rigidbody.velocity = BackUp();
            shouldMove = false;
        }
    }

    private Vector3 GiveChase() {
        Vector3 velocity;
        
        // Check in front of us for walls
        if (!Physics.Raycast(transform.position + raycastOffset, transform.forward, ObstacleDistance, layerMask, QueryTriggerInteraction.Ignore)) {
            velocity = transform.forward * speed;
            // We don't touch Y velocity to maintain integrity of gravity.
            return new Vector3(velocity.x, rigidbody.velocity.y, velocity.z);
        }

        // Don't walk directly into walls
        return new Vector3(0f, rigidbody.velocity.y, 0f);
    }

    private Vector3 MaintainDistance() {
        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        targetVelocity.y = 0;

        // Stop moving if target has stopped moving, otherwise continue to chase
        if (targetVelocity.magnitude == 0f) {
            return new Vector3(rigidbody.velocity.x * (1 - brakingSpeed / 100), rigidbody.velocity.y, rigidbody.velocity.z * (1 - brakingSpeed / 100));
        }

        return new Vector3(0f, rigidbody.velocity.y, 0f);
    }

    private Vector3 BackUp() {
        Vector3 velocity = -transform.forward * speed;
        // We don't touch Y velocity to maintain integrity of gravity.
        return new Vector3(velocity.x, rigidbody.velocity.y, velocity.z);
    }
}
