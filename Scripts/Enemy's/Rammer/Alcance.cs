using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcance : MonoBehaviour {

	[Header("Determina alcance do circle collider que detecta o player")]
	public float RaioDeAlcance;
	public Rammer ram;
	private bool dentroAlcance = false;

	// Use this for initialization
	void Start () {

		GetComponent<CircleCollider2D> ().radius = RaioDeAlcance;

	}
	
	// Update is called once per frame
	void Update () {

		ram.podeAlcancar = dentroAlcance;

	}

	void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") 
			dentroAlcance = true;
		
	}

	void OnTriggerStay2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") 
			dentroAlcance = true;
		
	}

	void OnTriggerExit2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") 
			dentroAlcance = false;
		
	}

}
