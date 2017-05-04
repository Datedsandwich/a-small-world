using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStealthController : MonoBehaviour {
	public bool isSneaking;

	void FixedUpdate () {
        // For now, only sets this bool, but this class will be responsible for making the player quiet
		isSneaking = Input.GetButton("Sneak");
	}
}
