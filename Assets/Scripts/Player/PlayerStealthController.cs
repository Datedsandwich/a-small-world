using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStealthController : MonoBehaviour {
    private PlayerMovement playerMovement;

	void Start () {
        playerMovement = GetComponent<PlayerMovement>();
	}
	
	void FixedUpdate () {
        bool isSneaking = Input.GetButton("Sneak");
        // For now, only sets this bool, but this class will be responsible for making the player quiet
        playerMovement.isSneaking = isSneaking;

        // Temporary visual for sneaking, as I have no models or animation yet.
        if(isSneaking) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(1, 2, 1);
        }
	}
}
