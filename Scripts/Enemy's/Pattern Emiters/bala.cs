using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour {

	[Header ("Força de Knock e dano")]
	public float dano;
	// dano que o rammer causa ao jogador
	public float forceX;
	// força em x do knock
	public float forceY;
	// força em y do knock
	public float speed;

	private Player playerScript;
	public GameObject particle;

	void Start(){
		Invoke ("autoDestruir", 10.0f);
	}

	// Use this for initialization
	void Update() {

		transform.SetParent (null);
		
//Acessando script Player
		if (playerScript == null) {
			playerScript = Global.PlayerAcess;
		}


		transform.Translate (Vector2.right*Time.deltaTime*speed);

	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Player") {
			damage ();
		} else if (col.gameObject.tag == "Chao" || col.gameObject.tag == "Respawn") {
			Instantiate (particle, transform.position, transform.rotation);

			Destroy (gameObject);
		}
			
	}
		
	void autoDestruir(){
		Instantiate (particle, transform.position, transform.rotation);

		Destroy (gameObject);
	}

	public void damage ()
	{
		Global.life -= dano;
		if (!Global.isKnock) {
			playerScript.StartCoroutine ("knockEnd");
			Global.isKnock = true;
			if (Global.playerRB.position.x > transform.position.x) {
				Global.playerRB.velocity = Vector2.zero;
				Global.playerRB.AddForce (new Vector2 (forceX, forceY), ForceMode2D.Force);
			} else if (Global.playerRB.position.x < transform.position.x) {
				Global.playerRB.velocity = Vector2.zero;
				Global.playerRB.AddForce (new Vector2 (forceX * -1, forceY), ForceMode2D.Force);
			}
		}
		Instantiate (particle, transform.position, transform.rotation);

		Destroy (gameObject);
	}
}
