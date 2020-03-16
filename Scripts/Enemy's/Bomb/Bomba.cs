using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour {

	[Header("Dano causado por cada fragmento")]
	public float dano;
	[Header("Tempo para explodir ao entrar no alcance")]
	public float tempoExplosao;
	[Header("Fragmento que será lançado ao explodir")]
	public GameObject Fragmento;
	[Header("Força de Knock dos Fragmentos")]
	public float forcaX, forcaY;
	[Header("Raio de alcance da bomba")]
	public float raio;

    //ARION OLHAR AQUI V V V
    private int animState = 0;
    // 0 = Repouso, 1 = Apitando


	private float distance;
	private bool jammerEfct, explodindo, explodir;
	private Player playerScript;
	public GameObject b00mfx;
    public AudioClip b00msound;
    public float b00mVolume = .3f;
    // public AudioClip hitsound;

    private Animator animBomba;

	// Use this for initialization
	void Start () {

		animBomba = GetComponent<Animator> ();
       
	}

    void Update() {
        if (playerScript == null)
        {
            playerScript = Global.PlayerAcess;
        }
		distance = Vector2.Distance(transform.position,playerScript.transform.position);

		if (explodir && !jammerEfct && !explodindo && !Global.pause) {
			explosao ();
		}

		if (distance < 8 && !IsInvoking("explosao")  && !Global.pause) {
            //animState = 1;
			animBomba.SetBool("isExploding",true);
			Invoke ("explosao", tempoExplosao);
		}
	}

	void explosao (){
        //Disparar animacao de explosao aqui
		if(Apolo.APOLOON){
			Apolo.quantidadeInimigosOn -= 1;
		}
		explodindo = true;
		Global.danoFrag = dano;
		Global.forceX = forcaX;
		Global.forceY = forcaY;
		for (int i = 0; i < 8; i++) {			
			Instantiate (Fragmento, transform.position, transform.rotation);
		}
		Instantiate (b00mfx, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(b00msound, playerScript.transform.position, b00mVolume);
        Destroy (gameObject);
	}

	public void atingidoJammer(float dmgBala){
		float vida = 0;
		jammerEfct = true;
		vida -= dmgBala;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (!jammerEfct) {
			switch (col.gameObject.tag) {
			case "Player":
				explodir = true;
				break;
			case "BalaP":
				explodir = true;
				break;
			case "Fragmento":
				explodir = true;
				break;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!jammerEfct) {
			switch (col.gameObject.tag) {
			case "Player":
				explodir = true;
				break;
			case "BalaP":
				explodir = true;
				break;
			case "Fragmento":
				explodir = true;
				break;
			}
		}
	}

}