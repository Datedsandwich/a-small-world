using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour {
    public float speed = 10;
    public float sneakSpeed = 6;
    public float jumpVelocity = 20;
    public float jumpReduction = 10;
    public Vector3 maxVelocityCap;
    public bool isSneaking;
    public bool isHandlingInput = true;
    public LayerMask layerMask;

    private new Rigidbody rigidbody;
    private new Collider collider;

    private Vector3 movement;
    public float height;
    private bool hasJumped = false;
    private bool cutJumpShort = false;
    private bool isGrounded;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void FixedUpdate() {
        CheckIfGrounded();
        ApplyJumpPhysics();
        ApplyMovementPhysics();
        CapVelocity();
    }

    private void ApplyMovementPhysics() {
        if (movement != Vector3.zero) {
            rigidbody.transform.rotation = Quaternion.LookRotation(movement);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        ManageMovement(horizontalInput, verticalInput);
    }

    private void CheckIfGrounded() {
        isGrounded = Physics.Raycast(transform.position, -transform.up, collider.bounds.extents.y, layerMask);
    }

    private void ManageMovement(float h, float v) {
        if (!isHandlingInput) {
            return;
        }

        Vector3 forwardMove = Vector3.Cross(Camera.main.transform.right, Vector3.up);
        Vector3 horizontalMove = Camera.main.transform.right;

        movement = forwardMove * v + horizontalMove * h;

        movement = isSneaking ? movement.normalized * sneakSpeed * Time.deltaTime : movement.normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }

    private void ApplyJumpPhysics() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpVelocity, rigidbody.velocity.z);
            hasJumped = false;
        }

        // Cancel the jump when the button is no longer pressed
        if (Input.GetButtonUp("Jump") && !isGrounded) {
            if (rigidbody.velocity.y > jumpReduction) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpReduction, rigidbody.velocity.z);
            }
            cutJumpShort = false;
        }
    }

    private void CapVelocity() {
        Vector3 _velocity = GetComponent<Rigidbody>().velocity;
        _velocity.x = Mathf.Clamp(_velocity.x, -maxVelocityCap.x, maxVelocityCap.x);
        _velocity.y = Mathf.Clamp(_velocity.y, -maxVelocityCap.y, maxVelocityCap.y);
        _velocity.z = Mathf.Clamp(_velocity.z, -maxVelocityCap.z, maxVelocityCap.z);
        rigidbody.velocity = _velocity;
    }
}