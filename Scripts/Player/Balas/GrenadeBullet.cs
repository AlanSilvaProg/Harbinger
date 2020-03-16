using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadaBala : MonoBehaviour {



	public float forcaX;
	public float forcaY;
	public float dmgBullet;
	public float explosionTime;
	private Rigidbody2D bulletRB;
	private float timer;
	private bool iniciaTimer;

	public GameObject deathParticle;
    //public AudioClip b00msound;
    //public float b00mVolume = 1f;

    // Use this for initialization
    void Start () {

		bulletRB = GetComponent<Rigidbody2D> ();

		timer = 0;

		Global.granadaNaCena = true;
		
		Global.balasNaCena[2] += 1;

		if(Global.lastLook == "dir" || Input.GetAxisRaw("Horizontal") > 0)
			bulletRB.AddForce(new Vector2(forcaX, forcaY));
		else
			if(Global.lastLook == "esq" || Input.GetAxisRaw("Horizontal") < 0)
				bulletRB.AddForce(new Vector2(forcaX * -1, forcaY));
			else
				if(Global.lastLook == "cim" || Input.GetAxisRaw("Vertical") > 0)
					bulletRB.AddForce(new Vector2(0, forcaY));
				else
					if(Global.lastLook == "bai" || Input.GetAxisRaw("Vertical") < 0)
						bulletRB.AddForce(new Vector2(0, forcaY * -1));
		
	}
	
	// Update is called once per frame
	void Update () {

		if (iniciaTimer)
			timer += Time.deltaTime;

		if (bulletRB.velocity.x > 0) {
			bulletRB.velocity = new Vector2 (bulletRB.velocity.x - 0.2f, bulletRB.velocity.y);
		}else if(bulletRB.velocity.x < 0) {
			bulletRB.velocity = new Vector2 (bulletRB.velocity.x + 0.2f, bulletRB.velocity.y);
		}
		
	}

	void OnColliderEnter2D(Collision2D col){
		switch(col.collider.gameObject.tag){
		case "Enemy":
			col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
			Global.bulletsOnScene [2] -= 1;
			Global.grenadeOnScene = false;
			PlayDeathParticle ();
			break;
		case "Plataforma":
			if (!iniciaTimer) {
				timer = 0;
				iniciaTimer = true;
			}
			break;
		case "Chao":
			if (!iniciaTimer) {
				timer = 0;
				iniciaTimer = true;
			}
			break;
		case "Obstaculo":
			col.gameObject.SendMessage("HittedGrenade", dmgBullet);
			Global.bulletsOnScene[2] -= 1;
			Global.grenadeOnScene = false;
			PlayDeathParticle ();
			break;
		case "Boss":
			col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
			Global.bulletsOnScene [2] -= 1;
			Global.grenadeOnScene = false;
			PlayDeathParticle ();
			break;
		}
	}
	void OnTriggerStay2D(Collider2D col){

		if (timer >= explosionTime) {
			switch (col.gameObject.tag) {
			case "Enemy":
				col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
				Global.bulletsOnScene [2] -= 1;
				Global.grenadeOnScene = false;
				PlayDeathParticle ();
				break;
			case "Plataforma":
				col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
				Global.bulletsOnScene [2] -= 1;
				Global.grenadeOnScene = false;
				PlayDeathParticle ();
				break;
			case "Chao":
				col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
				Global.bulletsOnScene [2] -= 1;
				Global.grenadeOnScene = false;
				PlayDeathParticle ();
				break;
			case "Obstaculo":
				col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
				Global.bulletsOnScene [2] -= 1;
				Global.grenadeOnScene = false;
				PlayDeathParticle ();
				break;
			case "Boss":
				col.gameObject.SendMessage ("HittedGrenade", dmgBullet);
				Global.bulletsOnScene [2] -= 1;
				Global.grenadeOnScene = false;
				PlayDeathParticle();
				break;
			}
		}
	}


	void PlayDeathParticle(){
		Instantiate (deathParticle, transform.position, transform.rotation);
       // AudioSource.PlayClipAtPoint(b00msound, balaRB.transform.position, b00mVolume);
        Destroy (gameObject);
	}

}
