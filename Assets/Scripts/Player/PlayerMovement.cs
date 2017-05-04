using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour {
    public float speed = 10;
    public float sneakSpeed = 6;
    public Vector3 maxVelocityCap;
    public bool isHandlingInput = true;
    public LayerMask layerMask;

    private new Rigidbody rigidbody;
    private Vector3 movement;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        CapVelocity();
    }

	public void ManageMovement(float horizontal, float vertical, bool isSneaking) {
        if (!isHandlingInput) {
            return;
        }

		if (movement != Vector3.zero) {
			rigidbody.transform.rotation = Quaternion.LookRotation(movement);
		}

        Vector3 forwardMove = Vector3.Cross(Camera.main.transform.right, Vector3.up);
        Vector3 horizontalMove = Camera.main.transform.right;

        movement = forwardMove * vertical + horizontalMove * horizontal;

        movement = isSneaking ? movement.normalized * sneakSpeed * Time.deltaTime : movement.normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }

    private void CapVelocity() {
        Vector3 _velocity = GetComponent<Rigidbody>().velocity;
        _velocity.x = Mathf.Clamp(_velocity.x, -maxVelocityCap.x, maxVelocityCap.x);
        _velocity.y = Mathf.Clamp(_velocity.y, -maxVelocityCap.y, maxVelocityCap.y);
        _velocity.z = Mathf.Clamp(_velocity.z, -maxVelocityCap.z, maxVelocityCap.z);
        rigidbody.velocity = _velocity;
    }
}