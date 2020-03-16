using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpColetavel : MonoBehaviour {

	[Header("Ativar o Power Up correspondente a esse coletavel")]
	public bool wallJump;
	public bool groundPound;
	public bool dash;
	public bool stasi;
	public bool doubleJump;
	public bool granada;
	public bool jammer;
    public Animator anim;
	[Header("Tempo necessario de scan para coletar")]
	public float tempoColetar;


	private float timer = 0;

	private bool escaneado;

	void Update(){
		if(wallJump && Global.wallJump){
			Destroy (gameObject);
			//anim.SetBool("Taken", true);
		}
		if(groundPound && Global.groundPound){
			Destroy (gameObject);
			//anim.SetBool("Taken", true);
		}
		if (dash && Global.dash){
			Destroy (gameObject);
			//anim.SetBool("Taken", true);
		}
		if (stasi && Global.stasi){
			Destroy (gameObject);
			//anim.SetBool("Taken", true);
		}
		if (doubleJump && Global.doubleJump){
			Destroy (gameObject);
			//anim.SetBool("Taken", true);
		}
		if (granada && Global.granada) {
			Destroy (gameObject);
		}
		if (jammer && Global.jammer) {
			Destroy (gameObject);
		}
	}

	void scanFim(){
		if(wallJump){
			Global.wallJump = true;
			LoadIt ();
		//anim.SetBool("Taken", true);
		}
		if(groundPound){
			Global.groundPound = true;
			LoadIt ();

		//anim.SetBool("Taken", true);
		}
		if (dash){
			Global.dash = true;
			LoadIt ();

		//anim.SetBool("Taken", true);
		}
		if (stasi){
			Global.stasi = true;
			LoadIt ();

		//anim.SetBool("Taken", true);
		}
		if (doubleJump){
			Global.doubleJump = true;
			LoadIt ();

		//anim.SetBool("Taken", true);
		}
		if (granada) {
			Global.granada = true;
			LoadIt ();

			//anim.SetBool("Taken", true);
		}
		if (jammer) {
			Global.jammer = true;
			LoadIt ();

			//anim.SetBool("Taken", true);
		}
		Destroy (gameObject);
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Scanner" && timer < tempoColetar && !escaneado) {
			timer += Time.deltaTime;
		} else if (col.gameObject.tag == "Scanner" && timer > tempoColetar && !escaneado) {
			scanFim ();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Scanner") {
			timer = 0;
		}
	}


	void LoadIt(){
		Global.playerRB.gravityScale = 0;
		Global.PlayerAcess.gameObject.GetComponent<Collider2D> ().enabled = false;
		Global.PlayerAcess.gameObject.transform.SetParent (null);
		DontDestroyOnLoad (Global.PlayerAcess.gameObject);
		Global.randomValue = (int)Random.Range (1, 7);
		Global.saida = 0;
		if (wallJump) {
			Loading.cenaACarregar = "Wall jump cutscene";
		} else if (jammer) {
			Loading.cenaACarregar = "Jammer Gun Cutscene";
		} else if (doubleJump) {
			Loading.cenaACarregar = "Double Jump Cutscene";
		} else if (dash) {
			Loading.cenaACarregar = "Dash Cutscene";
		} else if (granada) {
			Loading.cenaACarregar = "Grenade cutscene";
		}
		Loading.load = true;
	}
}
