using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTrigger : MonoBehaviour {

	[Header("Textos de tutorial do trigger em ordem")]
	public string[] tuto;

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

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			attTutos ();
			Global.GCAcess.gameObject.SendMessage ("tutoTrigger",new Vector2(0, tuto.Length -1));
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			attTutos ();
			Global.GCAcess.gameObject.SendMessage ("tutoTrigger",new Vector2(0, tuto.Length - 1));
			Destroy (gameObject);
		}
	}

}
