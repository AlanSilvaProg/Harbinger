using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basica : MonoBehaviour {

	public GameObject PFBala;
	public Transform pontosD;
	public float pausaEntreTiros, vida;
	private Rigidbody2D torretRB, playerRB;
	private float distancia;
	private bool travar, playerAlcance, atirando, esq = false, dir = true;
    public Animator animator;
    public ParticleSystem b00mfx;
    public AudioClip b00msound;
    public float b00mVolume = .05f;
    public AudioClip shotsound;
    public float shotVolume = .06f;
    // public AudioClip hitsound;


    // Use this for initialization
    void Start () {
        animator = animator.GetComponent<Animator>();
		playerRB = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();
		torretRB = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {


		/*
		if (playerRB.position.x < torretRB.position.x && esq == false) {
			flipEsq ();
		}

		if (playerRB.position.x > torretRB.position.x && dir == false) {
			flipDir ();
		}*/

        if (vida <= 0)
        {
            morte();
        }

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
        animator.SetTrigger("OnHit");
        //AudioSource.PlayClipAtPoint(hitsound, playerRB.transform.position, b00mVolume);
    }

	void atingidoJammer(float dmg){
		if (travar == false) {
			travar = true;
			//tocar som de torret sendo travada pela jammer
		}
		vida -= dmg;
	}

	/*void flipEsq (){

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

	}*/

	void morte(){
        Instantiate(b00mfx, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(b00msound, playerRB.transform.position, b00mVolume);
        Destroy (gameObject);
	}

	IEnumerator Shots(){
        animator.SetTrigger("OnShoot");
		Instantiate(PFBala, pontosD.position, pontosD.rotation);
        AudioSource.PlayClipAtPoint(b00msound, playerRB.transform.position, b00mVolume);
        yield return new WaitForSeconds (pausaEntreTiros);
		atirando = false;
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
