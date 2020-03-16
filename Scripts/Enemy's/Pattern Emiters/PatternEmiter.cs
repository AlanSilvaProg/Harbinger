using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternEmiter : MonoBehaviour {

	[Header("Vida do Pattern")]
	public float vida;
	[Header("PreFab da Bala")]
	public GameObject balaPF;
	[Header("Acionar apenas uma direção de Disparo")]
	public bool cima;
	public bool baixo;
	public bool direita;
	public bool esquerda;
	[Header ("Força de Knock e dano")]
	public float dano;
	// dano que o rammer causa ao jogador
	public float forceX;
	// força em x do knock
	public float forceY;
	// força em y do knock
	[Header ("Pause Entre Danos")]
	public float pausaDano;
	[Header("Pausa entre ataques")]
	public float tempoPausa;
	[Header("Tiros por ataque")]
	public int quantTiros;
	[Header("Padrão de tempo entre disparos")]
	public float timerDisparo;
	[Header("Ponto de Disparo")]
	public Transform pontoD;
	[Header("Linkar objeto filho com nome mira")]
	public Transform mira;
	[Header("Tempo paralizado pelo jammer")]
	public float tempoEfeitoJammer;
	[Header("SPREAD STUFF")]
	public bool isSpreadFire = false;
	public float spread = 0;
	public int spreadAmmount = 1;

	private bool travar;
	private Player playerScript;
	private bool disparando;
	private bool pauseDmg;
	private int controlTiro = 0;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (!Global.pause) {

			//Acessando script Player
			if (playerScript == null) {
				playerScript = Global.PlayerAcess;
			}

			if (!disparando && controlTiro == 0 && !travar) {
				StartCoroutine ("Disparo");
			}

			if (vida <= 0) {
				morto ();
			}
		}

	}

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.tag == "Player") {
			damage ();
		}

	}

	IEnumerator Disparo(){

		disparando = true;

		yield return new WaitForSeconds (timerDisparo);
		if (controlTiro < quantTiros) {
			if(!travar){
				if (isSpreadFire) {
					shootWithSpread ();
				} else {
					atirar ();
				}
			}
			StartCoroutine ("Disparo");
		} else {
			StartCoroutine ("Pausa");
		}

	}

	IEnumerator Pausa(){
		yield return new WaitForSeconds (tempoPausa);
		controlTiro = 0;
		disparando = false;
	}


	void morto(){
		if(Apolo.APOLOON){
			Apolo.quantidadeInimigosOn -= 1;
		}
		Destroy (gameObject);
	}

	void atirar(){

		controlTiro++;
		if (cima) {
			GameObject tempbala =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,90.0f+transform.eulerAngles.z));
		}
		if (direita) {
			GameObject tempbala =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,0.0f+transform.eulerAngles.z));
		}
		if (esquerda) {
			GameObject tempbala = Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,180.0f+transform.eulerAngles.z));
		}
		if (baixo) {
			GameObject tempbala = Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,-90.0f+transform.eulerAngles.z));
		}

	}



	public void atingidoBala(float dmgBala){
		vida -= dmgBala;
	}

	public void atingidoJammer(float dmgBala){
		if(!travar)
			StartCoroutine ("travaJammer");
		vida -= dmgBala;
	}

	public void damage ()
	{
		if (!pauseDmg) {
			StartCoroutine ("pauseToDmg");
			Global.life -= dano;
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
	}

	IEnumerator travaJammer(){
		travar = true;
		yield return new WaitForSeconds (tempoEfeitoJammer);
		travar = false;
	}

	IEnumerator pauseToDmg ()
	{
		pauseDmg = true;
		yield return new WaitForSeconds (pausaDano);
		GetComponent<CircleCollider2D> ().enabled = true;
		pauseDmg = false;
		Global.isKnock = false;
	}

	void shootWithSpread(){
		controlTiro++;
		if (cima) {
			GameObject tempbala = Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,90.0f+transform.eulerAngles.z));
			for (int i = 1; i <= spreadAmmount; i++) {
				GameObject tempbala2 =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala2.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,90.0f+transform.eulerAngles.z+spread*i));
				GameObject tempbala3 =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala3.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,90.0f+transform.eulerAngles.z-spread*i));
			}
		}
		if (direita) {
			GameObject tempbala =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,0.0f+transform.eulerAngles.z));
			for (int i = 1; i <= spreadAmmount; i++) {
				GameObject tempbala2 =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala2.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,0.0f+transform.eulerAngles.z+spread*i));
				GameObject tempbala3  = Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala3.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,0.0f+transform.eulerAngles.z-spread*i));
			}
		}
		if (esquerda) {
			GameObject tempbala = Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,180.0f+transform.eulerAngles.z));
			for (int i = 1; i <= spreadAmmount; i++) {
				GameObject tempbala2 =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala2.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,180.0f+transform.eulerAngles.z+spread*i));
				GameObject tempbala3 =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala3.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,180.0f+transform.eulerAngles.z-spread*i));
			}

		}
		if (baixo) {
			GameObject tempbala =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
			tempbala.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,-90.0f+transform.eulerAngles.z));
			for (int i = 1; i <= spreadAmmount; i++) {
				GameObject tempbala2 = Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala2.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,-90.0f+transform.eulerAngles.z+spread*i));
				GameObject tempbala3 =  Instantiate (balaPF, pontoD.transform.position, transform.rotation);
				tempbala3.transform.SetPositionAndRotation (tempbala.transform.position, Quaternion.Euler(0.0f,0.0f,-90.0f+transform.eulerAngles.z-spread*i));
			}

		}

	}

}
