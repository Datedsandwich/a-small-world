using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {
	public static bool RandomBool () {
		float num = Random.Range (0f, 1f);
		if (num >= 0.5) {
			return true;
		}
		return false;
	}
}
