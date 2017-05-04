using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour {
	public float damage;
	public GameObject attackText;

	public void Attack(PlayerHealth health) {
		health.Damage (damage);
	}

	void OnTriggerEnter (Collider other) {
		if(other.CompareTag(Tags.player)) {
			PlayerHealth health = other.GetComponent<PlayerHealth> ();

			if(health) {
				Attack (health);
				StartCoroutine (ShowText ());
			}
		}
	}

	private IEnumerator ShowText() {
		attackText.SetActive (true);
		yield return new WaitForSeconds (3.0f);
		attackText.SetActive (false);
	}
}
