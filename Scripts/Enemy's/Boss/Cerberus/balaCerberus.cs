using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaCerberus : MonoBehaviour {
	[Header("Velocidade da bala")]
	public float veloBala; // velocidade em que a bala se movimentará para a frente
	[Header("dano da bala")]
	public float dano; // dano que ela causará ao jogador
	private Player playerScript; // acesso ao script do player
	public GameObject particle;
	// Use this for initialization
	void Start () {

//dando unparent para a bala agir sozinha
		transform.SetParent (null);

//Guardando acesso do player script
		playerScript = Global.PlayerAcess;

	}
	
	// Update is called once per frame
	void Update () {
//movendo ela na direção right do vector (Rotação ja é ajustada no pre fab de balas do boss, e também no stantiate do Cerberus)	
		transform.Translate (Vector2.right * Time.deltaTime * veloBala);
		
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Player") {

			//Global.life -= dano;
			Global.PlayerAcess.SendMessage("atingido", dano);
			if (!Global.isKnock) {
				Global.PlayerAcess.StartCoroutine ("knockEnd");
				Global.isKnock = true;
				if (Global.playerRB.position.x > transform.position.x) {
					Global.playerRB.velocity = new Vector2 (0, 0);
					Global.playerRB.AddForce (new Vector2 (Global.forceX, Global.forceY), ForceMode2D.Force);
				} else if (Global.playerRB.position.x < transform.position.x) {
					Global.playerRB.velocity = new Vector2 (0, 0);
					Global.playerRB.AddForce (new Vector2 (Global.forceX * -1, Global.forceY), ForceMode2D.Force);
				}
			}
			Global.quantFrag--;
			Instantiate (particle, transform.position, transform.rotation);

			Destroy (gameObject);
		}
		if (col.gameObject.tag == "Chao") {
			Global.quantFrag--;
			Instantiate (particle, transform.position, transform.rotation);

			Destroy (gameObject);
		}
	}

}
