using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentroBala : MonoBehaviour {

	public Rigidbody2D TbalaRB;
	public float dmg, tempOn, forceX, forceY;
	private Player playerScript;
	private bool deudano = false;

	// Use this for initialization
	void Start () {

		playerScript = FindObjectOfType (typeof(Player)) as Player;
		StartCoroutine ("centerShot");

	}

	void damage(float dmg){
		playerScript.atingido(dmg);
		if (!Global.isKnock) {
			playerScript.StartCoroutine ("knockEnd");
			Global.isKnock = true;
			if(Global.playerRB.position.x > transform.position.x)
				Global.playerRB.AddForce (new Vector2 (forceX, forceY), ForceMode2D.Force);
			if(Global.playerRB.position.x < transform.position.x)
				Global.playerRB.AddForce (new Vector2 (forceX * -1, forceY), ForceMode2D.Force);
		}
	}

	IEnumerator centerShot(){
		yield return new WaitForSeconds (tempOn);
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player" && !deudano) {
			deudano = true;
			damage (dmg);
		}

	}
	void OnTriggerStay2D(Collider2D col){

		if (col.gameObject.tag == "Player" && !deudano) {
			deudano = true;
			damage (dmg);
		}

	}

}