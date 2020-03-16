using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaColetavel : MonoBehaviour {
    public Animator animator;

	[Header("Valor em % de ganho de vida ao coletar")]
	public float valorVidaAdc;
	[Header("Tempo necessario de scan para coletar")]
	public float tempoColetar;
	public bool isRecover = false;

	private float timer = 0;

	private bool escaneado;

	void scanFim(){
		if (isRecover) {
			Global.life += valorVidaAdc;
			if (Global.life > Global.lifeMax) {
				Global.life = Global.lifeMax;
			}
		}	
	}

	void OnTriggerStay2D(Collider2D col){
        animator.SetTrigger("TakeOn");
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

}
