using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour {

	public float bulletSpeed;
	public float dmgBullet;
	public int distanceMax;
	private Rigidbody2D bulletRB;
	private Transform cameraT;

	public GameObject deathParticle;

	// Use this for initialization
	void Start () {

		bulletRB = GetComponent<Rigidbody2D> ();

		cameraT = GameObject.Find ("RefBullet").GetComponent<Transform> ();

		Global.balasNaCena[0] += 1;

		if (Input.GetAxisRaw ("Vertical") > 0) {
			bulletRB.velocity = new Vector2 (0, bulletSpeed);
		} else if (Input.GetAxisRaw ("Vertical") < 0) {
			bulletRB.velocity = new Vector2 (0, bulletSpeed * -1);
		} else if (Global.lastLook == "dir" || Input.GetAxisRaw ("Horizontal") > 0) {
			bulletRB.velocity = new Vector2 (bulletSpeed, 0);
		} else if (Global.lastLook == "esq" || Input.GetAxisRaw ("Horizontal") < 0) {
			bulletRB.velocity = new Vector2 (bulletSpeed * -1, 0);
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (bulletRB.position.x > cameraT.position.x + distanceMax || bulletRB.position.x < cameraT.position.x - distanceMax){
			Global.bulletsOnScene[0] -= 1;
			PlayDeathParticle ();
		}
		if (bulletRB.position.y > cameraT.position.y + distanceMax || bulletRB.position.y < cameraT.position.y - distanceMax){
			Global.bulletsOnScene[0] -= 1;
			PlayDeathParticle ();
		}

}

	void OnTriggerEnter2D(Collider2D col){

		switch(col.gameObject.tag){
		case "Enemy":
			col.gameObject.SendMessage("hittedBullet", dmgBullet);
			Global.bulletsOnScene[0] -= 1;
			PlayDeathParticle ();		
			break;
		case "Chao":
			Global.bulletsOnScene[0] -= 1;
			PlayDeathParticle ();		
			break;
		}
	}

	void PlayDeathParticle(){
		Instantiate (deathParticle, transform.position, transform.rotation);
		Destroy (gameObject);
	}

}