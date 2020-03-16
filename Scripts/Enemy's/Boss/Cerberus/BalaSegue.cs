using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaSegue : MonoBehaviour {

	[Header("Velocidade da Bala")]
	public float veloBala; // velocidade da bala
	[Header("Valores de curva")]
	public float valorCurva; // valor que será somado em suas curvas para o jogador
	[Header("Dano da bala")]
	public float dano; // dano que causara ao jogador
	private Vector3 alvo; // alvo para onde a bala irá se referenciar na movimentação
	private Player playerScript; // acesso do script do player
	private bool attControl = false; // Variavel de controle interno, usado em testes e mantido para atualização unica em certos pontos
	private bool dir; // Direção que a bala irá se mover
	private float lifetime = 0;
	public GameObject particle;

	// Use this for initialization
	void Start () {
//Unparent para a bala agir por si só
		transform.SetParent (null);
//Guardando informações do alvo
		alvo = Global.alvo;

	}
	
	// Update is called once per frame
	void Update () {
		lifetime += 1 * Time.deltaTime;
		if (lifetime >= 8) {
			Instantiate (particle, transform.position, transform.rotation);

			Destroy (gameObject);
		}
//Atualizando informações do alvo
		alvo = Global.alvo;
//Atualização necessária apenas uma vez no update, uso do Control para verificar a direção que a bala irá se mover
		/*if (!attControl) {
			if (transform.position.x > alvo.x) {
				dir = true;
			}
			attControl = true;
		}

// Verificando e movendo para a direção correta
		if (dir) { // Verificando e movendo para a direção correta (Direita)
			transform.Translate (Vector2.left * Time.deltaTime * veloBala);
		} else {// Verificando e movendo para a direção correta (Direita)
			transform.Translate (Vector2.right * Time.deltaTime * veloBala);
		}// Verificando e movendo para a altura correta
		if (transform.position.y < alvo.y - 0.2f) {// Verificando e movendo para a altura correta (Cima)
			transform.Translate (Vector2.up * Time.deltaTime * valorCurva);
		} else if (transform.position.y > alvo.y + 0.2f) {// Verificando e movendo para a altura correta (baixo)
			transform.Translate (Vector2.down * Time.deltaTime * valorCurva);
		}*/

		OlharAlvoLazor ();
		transform.Translate (veloBala * Time.deltaTime, 0, 0);

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
		//	print ("AAAAAAAAAAAAAAAAAAAAAA");
			Instantiate (particle, transform.position, transform.rotation);

			Destroy (gameObject);
		}
		if (col.gameObject.tag == "Chao") {
			Global.quantFrag--;
			Instantiate (particle, transform.position, transform.rotation);

			Destroy (gameObject);
		}
	}


	void OlharAlvoLazor ()
	{


		Vector3 difference = alvo - transform.position;
		float rotationZ = (Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg);
		rotationZ = (Mathf.MoveTowardsAngle(transform.eulerAngles.z,rotationZ,80*Time.deltaTime));
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);
		//print (transform.eulerAngles);

	}

}
