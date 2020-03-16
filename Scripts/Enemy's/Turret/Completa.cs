using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Completa : MonoBehaviour {

	public GameObject[] PFBala;
	public Transform[] pontosD;
	public bool modoAgressivo;
	public float tempoAgressivo, pausaEntreTiros, vida;
	private Rigidbody2D torretRB, playerRB;
	private bool travar, playerAlcance, atirando, esq = false, dir = true;

	// Use this for initialization
	void Start () {

		playerRB = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();
		torretRB = GetComponent<Rigidbody2D> ();

		if (modoAgressivo == true)
			StartCoroutine ("centerShot");

	}
	
	// Update is called once per frame
	void Update () {

		if (playerRB.position.x < torretRB.position.x && esq == false) {
			flipEsq ();
		}

		if (playerRB.position.x > torretRB.position.x && dir == false) {
			flipDir ();
		}

		if (vida <= 0)
			morte ();

		if (!travar && atirando == false && playerAlcance == true && !Global.pause)
			atirar ();
		else if (travar || Global.pause) {
			atirando = false;
			StopCoroutine ("Shots");
		}


	}

	void atirar(){
		atirando = true;
		StartCoroutine ("Shots");
	}

	void atingidoBala(float dmg){
		vida -= dmg;
	}

	void atingidoJammer(float dmg){
		if (travar == false) {
			travar = true;
			//tocar som de torret sendo travada pela jammer
		}
		vida -= dmg;
	}

	void flipEsq (){

		dir = false;
		esq = true;
		
		Vector3 theScale = transform.localScale;
		if(theScale.x > 0)
			theScale.x *= -1;
		transform.localScale = theScale;

	}
	void flipDir (){

		dir = true;
		esq = false;

		Vector3 theScale = transform.localScale;
		if(theScale.x < 0)
			theScale.x *= -1;
		transform.localScale = theScale;

	}

	void morte(){
		//tocar som de torret sendo destruida e tocar animações necessárias
		Destroy (gameObject);
	}

	IEnumerator Shots(){
		int bala = (int)Random.Range (0, 2);

		Instantiate(PFBala[bala], pontosD[bala].position, pontosD[bala].rotation);
		yield return new WaitForSeconds (pausaEntreTiros);
		atirando = false;
	}

	IEnumerator centerShot(){
		yield return new WaitForSeconds (tempoAgressivo);
		if (playerAlcance && !travar && dir && !esq) 
			Instantiate (PFBala [2], new Vector3 (pontosD [2].position.x + PFBala [2].transform.localScale.x / 2 - 0.5f, pontosD [2].position.y, pontosD [2].position.y), pontosD [2].rotation);
		if (playerAlcance && !travar && !dir && esq) 
			Instantiate (PFBala [2], new Vector3 (pontosD [2].position.x - PFBala [2].transform.localScale.x / 2 + 0.5f, pontosD [2].position.y, pontosD [2].position.y), pontosD [2].rotation);
		yield return new WaitForSeconds (tempoAgressivo);
		StartCoroutine ("centerShot");
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player")
			playerAlcance = true;
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player")
			playerAlcance = false;
	}

}