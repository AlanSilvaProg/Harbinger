using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternEsq : MonoBehaviour {

	private float timer = 0;

	void Update () {

		if (timer < Apolo.OrbTime) {
			timer += Time.deltaTime;
		} else if (timer > Apolo.OrbTime) {
			Apolo.emissorE = false;
			Destroy (gameObject);
		}


	}
}