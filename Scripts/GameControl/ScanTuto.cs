using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTuto : MonoBehaviour {

	[Header("Textos de tutorial do trigger em ordem")]
	public string[] tuto;
	[Header("Tempo necessario de scan para coletar")]
	public float tempoColetar;

	private float timer = 0;

	private bool escaneado;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void attTutos(){
		for (int i = 0; i < tuto.Length; i++) {
			if (i >= Global.tuto.Length) {
				Global.tuto = new string[i + 1];
			}
			Global.tuto [i] = tuto [i];
		}
	}

	void scanFim(){

		attTutos ();
		Global.GCAcess.gameObject.SendMessage ("tutoTrigger",new Vector2(0, tuto.Length -1));
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