using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipApolo : MonoBehaviour {

	private Vector3 theScale;
	private Player playerScript;
	
	// Update is called once per frame
	void Update () {

		if (playerScript == null) {
			playerScript = Global.PlayerAcess;
		}

		if (playerScript.transform.position.x > gameObject.transform.position.x && transform.localScale.x < 0) {
			theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		if (playerScript.transform.position.x < gameObject.transform.position.x && transform.localScale.x > 0) {
			theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}


	}
}
