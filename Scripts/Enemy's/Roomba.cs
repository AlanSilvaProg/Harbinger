using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomba : MonoBehaviour {

	public Rigidbody2D roombaRB;
	public Transform pointA, pointB;
	public float dmg, tempoProxDmg, tempoEfeitoJammer, veloRoomba, forceX, forceY;
	public float life;
	private Player playerScript;
	private bool pauseDmg;
	private string destino;
	private Vector3 goRoombaD;
	private bool goRoombaA, goRoombaB, travar;
    public Animator animator;
    public ParticleSystem b00mfx;
    public AudioClip b00msound;
    public float b00mVolume = .3f;
    // public AudioClip hitsound;


    // Use this for initialization
    void Start () {

		playerScript = FindObjectOfType(typeof(Player)) as Player;
        animator = animator.GetComponent<Animator>();
		goRoombaA = true;
		goRoombaB = false;
		travar = false;
		StartCoroutine ("Kine");


	}
	
	// Update is called once per frame
	void Update () {

        if (life <= 0)
        {
            Instantiate(b00mfx, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(b00msound, playerScript.transform.position, b00mVolume);
			if(Apolo.APOLOON){
				Apolo.quantidadeInimigosOn -= 1;
			}
            Destroy(gameObject);
        }

		if (goRoombaA == true) {
			destino = "A";
			goRoombaD = new Vector2 (pointA.position.x, roombaRB.position.y);
		}
		if (goRoombaB == true) {
			destino = "B";
			goRoombaD = new Vector2 (pointB.position.x, roombaRB.position.y);
		}

		if (travar == true) {
			roombaRB.position = Vector3.MoveTowards (roombaRB.position, roombaRB.position, 0);
		}
		if (!Global.pause) {
			switch (destino) {
			case "A":
			
				if (roombaRB.position.x >= pointA.position.x - 1) {
					goRoombaA = false;
					goRoombaB = true;
				} else if (goRoombaA == true && roombaRB.position.x != pointA.position.x + 1 && travar == false) {
					roombaRB.position = Vector2.MoveTowards (roombaRB.position, goRoombaD, veloRoomba * Time.deltaTime);
				}
				
				break;
			case "B":
			
				if (roombaRB.position.x <= pointB.position.x + 1) {
					goRoombaB = false;
					goRoombaA = true;
				} else if (goRoombaB == true && roombaRB.position.x != pointB.position.x && travar == false) {
					roombaRB.position = Vector3.MoveTowards (roombaRB.position, goRoombaD, veloRoomba * Time.deltaTime);
				}

				break;
			}

		}
	}

	public void damage(){
		if (pauseDmg == false) {
			StartCoroutine ("pauseToDmg");
            //Global.life -= dmg;
            Global.PlayerAcess.SendMessage("atingido", dmg);
			playerScript.StartCoroutine ("knockEnd");
			Global.isKnock = true;
			if (Global.playerRB.position.x > roombaRB.position.x) {
				Global.playerRB.velocity = new Vector2(0,0);
				Global.playerRB.AddForce (new Vector2 (forceX, forceY), ForceMode2D.Force);
			} else if (Global.playerRB.position.x < roombaRB.position.x) {
				Global.playerRB.velocity = new Vector2(0,0);
				Global.playerRB.AddForce (new Vector2 (forceX * -1, forceY), ForceMode2D.Force);
			}
		}
	}

	public void atingidoBala(float dmgBala){
		life -= dmgBala;
        animator.SetTrigger("OnHit");
        //AudioSource.PlayClipAtPoint(hitsound, Global.playerRB.transform.position, b00mVolume);
    }

    public void atingidoJammer(float dmgBala){
		if(!travar)
			StartCoroutine ("travaJammer");
		life -= dmgBala;
	}

	IEnumerator pauseToDmg(){
		pauseDmg = true;
		yield return new WaitForSeconds (tempoProxDmg);
		pauseDmg = false;
		Global.isKnock = false;
	}

	IEnumerator travaJammer(){
		travar = true;
		yield return new WaitForSeconds (tempoEfeitoJammer);
		travar = false;
	}

	IEnumerator Kine(){
		yield return new WaitForSeconds (0.5f);
		roombaRB.isKinematic = true;
	}

	void OnCollisionEnter2D(Collision2D col){

		switch (col.gameObject.tag) {
		case "Player":
			damage ();
			break;
		}

	}
	void OnCollisionStay2D(Collision2D col){

		switch (col.gameObject.tag) {
		case "Player":
			damage ();
			break;
		}

	}
}
