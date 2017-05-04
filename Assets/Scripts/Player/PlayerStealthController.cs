using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStealthController : MonoBehaviour {
	public bool isSneaking;

	void FixedUpdate () {
        // For now, only sets this bool, but this class will be responsible for making the player quiet
		isSneaking = Input.GetButton("Sneak");

        // Temporary visual for sneaking, as I have no models or animation yet.
        if(isSneaking) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(1, 2, 1);
        }
	}
}
