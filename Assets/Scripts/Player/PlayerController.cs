using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerStealthController), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour {
	private PlayerMovement movement;
	private PlayerStealthController stealth;
	private PlayerHealth playerHealth;

	void Start() {
		movement = GetComponent<PlayerMovement> ();
		stealth = GetComponent<PlayerStealthController> ();
		playerHealth = GetComponent<PlayerHealth> ();
	}

	void FixedUpdate() {
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		movement.ManageMovement(horizontalInput, verticalInput, stealth.isSneaking);

		if(playerHealth.health <= 0) {
			Die ();
		} else {
			// Temporary visual for sneaking, as I have no models or animation yet.
			if(stealth.isSneaking) {
				transform.localScale = new Vector3(1, 1, 1);
			} else {
				transform.localScale = new Vector3(1, 2, 1);
			}
		}
	}

	void Die () {
		movement.isHandlingInput = false;
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3 (3f, 0.01f, 3f), Time.deltaTime * 2f);
	}
}
