using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpable : MonoBehaviour {
	
	private Transform platTransf;
	private Transform playerTransf;
	private BoxCollider2D platCol;
	private Player playerScript;
	private float timer;
	private bool getOut;

	// Use this for initialization
	void Start () {

		platTransf = transform;
		platCol = GetComponent<BoxCollider2D> ();
		getOut = false;
		timer = 0;

	}
	
	// Update is called once per frame
	void Update () {

		playerTransf = (FindObjectOfType(typeof(Player)) as Player).transform;

		playerScript = FindObjectOfType(typeof(Player)) as Player;

		if(getOut && timer >= 1) getOut = false;

		if (getOut)	timer += Time.deltaTime;

//Situações de ativar e desativar colisor da plataforma

		if (playerTransf.position.y - playerTransf.localScale.y/2 > platTransf.position.y + platTransf.localScale.y / 2 && !getOut && 
			playerScript.playerRB.velocity.y <= 0) {
			platCol.enabled = true;
		} else {	platCol.enabled = false;	}

		if (Input.GetKeyDown ("space") && Input.GetAxisRaw ("Vertical") < 0 && platCol.enabled == true && !getOut) {
			timer = 0;
			getOut = true;
			platCol.enabled = false;
		}
	}

}
