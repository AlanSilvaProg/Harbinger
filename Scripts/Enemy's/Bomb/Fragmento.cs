using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmento : MonoBehaviour {

	private int rotacaoFrag = -45;
	private int multiplosRotacao = 0;

	private float dano;

	private Player playerScript;

	// Use this for initialization
	void Start () {

		playerScript = Global.PlayerAcess;
		
		Global.quantFrag++;
		for (int i = 0; i < Global.quantFrag; i++) {
			multiplosRotacao++;
		}

		for (int i = 0; i < multiplosRotacao; i++) {
			rotacaoFrag += 45;
		}
			
		transform.Rotate (0, 0, rotacaoFrag);

		dano = Global.danoFrag;

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, 0.5f, 0);
	}

	void OnTriggerEnter2D(Collider2D col){
        
		if (col.gameObject.tag == "Player") {
			Global.PlayerAcess.SendMessage("atingido", dano);
			if (!Global.isKnock) {
				playerScript.StartCoroutine ("knockEnd");
				Global.isKnock = true;
				if (Global.playerRB.position.x > transform.position.x)
					Global.playerRB.AddForce (new Vector2 (Global.forceX, Global.forceY), ForceMode2D.Force);
				else if (Global.playerRB.position.x < transform.position.x)
					Global.playerRB.AddForce (new Vector2 (Global.forceX * -1, Global.forceY), ForceMode2D.Force);
			}
			Global.quantFrag--;
			Destroy (gameObject);
		}
		if (col.gameObject.tag == "Chao") {
			Global.quantFrag--;
			Destroy (gameObject);
		}
	}

}
