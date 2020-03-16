using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

	public bool Instakill; 
	public float dmg;
	[Header("Forças do knock")]
	public float forcaX;
	public float forcaY;
	private Player playerScript;

	void Start(){
		playerScript = FindObjectOfType (typeof(Player)) as Player;
	}

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.name == "Player"){
			if (Instakill) {
				Global.life = 0;
				//tocar som de morte
			} else {
				Global.life -= dmg;
				playerScript.StartCoroutine ("knockEnd");
				Global.isKnock = true;
				if (playerScript.playerRB.position.x < transform.position.x) {
					playerScript.playerRB.velocity = Vector2.zero;
					Global.playerRB.AddForce (new Vector2 (forcaX * -1, forcaY), ForceMode2D.Force);
				} else 	if (playerScript.playerRB.position.x > transform.position.x) {
					playerScript.playerRB.velocity = Vector2.zero;
					Global.playerRB.AddForce (new Vector2 (forcaX, forcaY), ForceMode2D.Force);
				}
			}
		}

	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.name == "Player"){
			if (Instakill) {
				Global.life = 0;
				//tocar som de morte
			} else {
				Global.life -= dmg;
				playerScript.StartCoroutine ("knockEnd");
				Global.isKnock = true;
				if (playerScript.playerRB.position.x < transform.position.x) {
					playerScript.playerRB.velocity = Vector2.zero;
					Global.playerRB.AddForce (new Vector2 (forcaX * -1, forcaY), ForceMode2D.Force);
				} else 	if (playerScript.playerRB.position.x > transform.position.x) {
					playerScript.playerRB.velocity = Vector2.zero;
					Global.playerRB.AddForce (new Vector2 (forcaX, forcaY), ForceMode2D.Force);
				}
			}
		}

	}
}