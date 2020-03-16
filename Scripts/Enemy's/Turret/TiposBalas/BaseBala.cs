using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBala : MonoBehaviour {
	
	public Rigidbody2D TbalaRB;
	public float velo, dmg, forceX, forceY;
	private Player playerScript;
	private Vector3 destino;

	// Use this for initialization
	void Start () {

		playerScript = FindObjectOfType (typeof(Player)) as Player;

		destino = Global.playerRB.position;

		if (Global.playerRB.position.x < TbalaRB.position.x)
			destino.x -= 4;
		else
			destino.x += 4;

	}

	// Update is called once per frame
	void Update () {

		TbalaRB.position = Vector3.MoveTowards (TbalaRB.position, destino, velo*Time.deltaTime);

		if (TbalaRB.position.x < destino.x + 0.2 && TbalaRB.position.x > destino.x - 0.2)
			Destroy (gameObject);

	}

	void damage(float dmg){
		playerScript.atingido(dmg);
		playerScript.StartCoroutine ("knockEnd");
		Global.isKnock = true;
		if (Global.playerRB.position.x > TbalaRB.position.x) {
			Global.playerRB.velocity = new Vector2 (0, 0);
			Global.playerRB.AddForce (new Vector2 (forceX, forceY), ForceMode2D.Force);
		} else if (Global.playerRB.position.x < TbalaRB.position.x) {
			Global.playerRB.velocity = new Vector2 (0, 0);
			Global.playerRB.AddForce (new Vector2 (forceX * -1, forceY), ForceMode2D.Force);
		}
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){

		switch (col.gameObject.tag) {
		case "Player":
			damage (dmg);
			break;
		case"Chao":
			Destroy (gameObject);
			break;
		case"Plataforma":
			Destroy (gameObject);
			break;
		}

	}
	void OnTriggerStay2D(Collider2D col){

		switch (col.gameObject.tag) {
		case "Player":
			damage (dmg);
			break;
		case"Chao":
			Destroy (gameObject);
			break;
		case"Plataforma":
			Destroy (gameObject);
			break;
		}

	}

}
