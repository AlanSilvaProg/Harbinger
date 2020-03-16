using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDir : MonoBehaviour {

	private float timer = 0 ;

	void Update () {

		if (timer < Apolo.OrbTime) {
			timer += Time.deltaTime;
		} else if (timer > Apolo.OrbTime) {
			Apolo.emissorD = false;
			Destroy (gameObject);
		}


	}
}
