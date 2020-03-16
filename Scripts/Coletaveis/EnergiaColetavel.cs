using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergiaColetavel : MonoBehaviour {
    public Animator animator;
    [Header("Valor em % de ganho de Energia ao coletar")]
	public float valorEnergiaAdc;
	[Header("Tempo necessario de scan para coletar")]
	public float tempoColetar;

	private float timer = 0;

	private bool escaneado;


	void scanFim(){
		Global.Energia += valorEnergiaAdc;
        animator.SetTrigger("TakeOn");
        
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Scanner" && timer < tempoColetar && !escaneado) {
			timer += Time.deltaTime;
		} else if (col.gameObject.tag == "Scanner" && timer > tempoColetar && !escaneado) {
			escaneado = true;
			scanFim ();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Scanner") {
			timer = 0;
		}
	}
}
