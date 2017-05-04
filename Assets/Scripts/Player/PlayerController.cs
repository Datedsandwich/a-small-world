using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerStealthController), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour {
	private PlayerMovement movement;
	private PlayerStealthController stealth;

	void Start() {
		movement = GetComponent<PlayerMovement> ();
		stealth = GetComponent<PlayerStealthController> ();
	}

	void FixedUpdate() {
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		movement.ManageMovement(horizontalInput, verticalInput, stealth.isSneaking);
	}
}
