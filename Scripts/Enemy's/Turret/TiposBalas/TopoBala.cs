using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopoBala : MonoBehaviour {

	public Rigidbody2D TbalaRB;
	public float velo, dmg, forceX, forceY;
	private Player playerScript;
	private Vector3 destino;
	private float tempAngle;
	public GameObject particle;

	// Use this for initialization
	void Start () {

		playerScript = FindObjectOfType (typeof(Player)) as Player;
		
		destino = GameObject.Find("Alvo").transform.position;

		/*if (Global.playerRB.position.x < TbalaRB.position.x)
			destino.x -= 4;
		else
			destino.x += 4;*/
		float ydif = Mathf.Abs (transform.position.y - destino.y);
		if (ydif < 1 && ydif > 0) {
			tempAngle = 0;

		} else {
			
			tempAngle = (Mathf.Abs (transform.position.x - destino.x) / Mathf.Abs (transform.position.y - destino.y));
		}
		Vector3 difference = destino - transform.position;
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);

	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate ((velo) * Time.deltaTime, (velo * tempAngle * Time.deltaTime) * Time.deltaTime, 0);
		//TbalaRB.position = Vector3.MoveTowards (TbalaRB.position, destino, velo*Time.deltaTime);
		//print (velo * tempAngle );
		/*ff (TbalaRB.position.x < destino.x + 0.2 && TbalaRB.position.x > destino.x - 0.2)
			Destroy (gameObject);
*/
	}

	void damage(float dmg){
		playerScript.atingido(dmg);
		playerScript.StartCoroutine ("knockEnd");
		Global.isKnock = true;
		if (Global.playerRB.position.x > TbalaRB.position.x)
			Global.playerRB.AddForce (new Vector2 (forceX,forceY), ForceMode2D.Force);
		else 	if (Global.playerRB.position.x < TbalaRB.position.x)
			Global.playerRB.AddForce (new Vector2 (forceX*-1,forceY), ForceMode2D.Force);
		Instantiate (particle, transform.position, transform.rotation);
		Destroy (gameObject);

	}

	void OnTriggerEnter2D(Collider2D col){

		switch (col.gameObject.tag) {
		case "Player":
			damage (dmg);
			break;
		case"Chao":
			Instantiate (particle, transform.position, transform.rotation);
			Destroy (gameObject);
			break;
		//case"Plataforma":
			//Destroy (gameObject);
			//break;
		}

	}
	void OnTriggerStay2D(Collider2D col){
		
		switch (col.gameObject.tag) {
		case "Player":
			damage (dmg);
			break;
		case"Chao":
			Instantiate (particle, transform.position, transform.rotation);
			Destroy (gameObject);
			break;
		//case"Plataforma":
		//	Destroy (gameObject);
			//break;
		}

	}

}
