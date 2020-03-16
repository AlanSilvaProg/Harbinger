using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaneavelLore : MonoBehaviour {

	[Header("Titulo do conteudo apresentado no Log")]
	public string Titulo;
	[Header("Conteudo exibido no log")]
	public string Conteudo;
	[Header("Tempo necessario de scan para coletar")]
	public float tempoColetar;

	private float timer = 0;

	private bool escaneado;


	void scanFim(){

		PauseScript.titulos.Add (Titulo.ToString());
		PauseScript.conteudos.Add (Conteudo.ToString());
		PauseScript.warningHudStatic.SetActive (true);
		Destroy (gameObject);

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
