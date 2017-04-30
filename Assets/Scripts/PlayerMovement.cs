using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10;
    public float jumpVelocity = 20;
    public float jumpReduction = 10;
    public Vector3 maxVelocityCap;
    public bool isHandlingInput = true;

    private new Rigidbody rigidbody;
    public LayerMask layerMask;
    private Vector3 movement;
    private bool hasJumped = false;
    private bool cutJumpShort = false;
    private bool isGrounded;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        ApplyJumpPhysics();
        CapVelocity();
    }

    void Update() {
        if (movement != Vector3.zero) {
            rigidbody.transform.rotation = Quaternion.LookRotation(movement);
        }

        if (Physics.CheckSphere(transform.position, 1f, layerMask)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        ManageMovement(horizontalInput, verticalInput);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            Jump();
        }

        if (Input.GetButtonUp("Jump") && !isGrounded) {
            CutJumpShort();
        }
    }

    public void ManageMovement(float h, float v) {
        if (!isHandlingInput) {
            return;
        }

        Vector3 forwardMove = Vector3.Cross(Camera.main.transform.right, Vector3.up);
        Vector3 horizontalMove = Camera.main.transform.right;

        movement = forwardMove * v + horizontalMove * h;

        movement = movement.normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }

    public void Jump() {
        hasJumped = true;
    }

    public void CutJumpShort() {
        cutJumpShort = true;
    }

    private void ApplyJumpPhysics() {
        if (hasJumped) {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpVelocity, rigidbody.velocity.z);
            hasJumped = false;
        }

        // Cancel the jump when the button is no longer pressed
        if (cutJumpShort) {
            if (rigidbody.velocity.y > jumpReduction) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpReduction, rigidbody.velocity.z);
            }
            cutJumpShort = false;
        }
    }

    void CapVelocity() {
        Vector3 _velocity = GetComponent<Rigidbody>().velocity;
        _velocity.x = Mathf.Clamp(_velocity.x, -maxVelocityCap.x, maxVelocityCap.x);
        _velocity.y = Mathf.Clamp(_velocity.y, -maxVelocityCap.y, maxVelocityCap.y);
        _velocity.z = Mathf.Clamp(_velocity.z, -maxVelocityCap.z, maxVelocityCap.z);
        rigidbody.velocity = _velocity;
    }
}