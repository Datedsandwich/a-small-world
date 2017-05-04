using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
	public float health { get; private set;}
	public float maxHealth = 100;

	void Start() {
		health = maxHealth;
	}

	public void Damage(float value) {
		health -= value;
		health = health > 0 ? health : 0;
	}

	public void Heal(float value) {
		health += value;
		health = health < maxHealth ? health : maxHealth;
	}
}
