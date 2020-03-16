using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holografic : MonoBehaviour {

	[Header("Tempo que ficará desativado")]
	public float tempoOff;
	[Header("Tempo que ficará ativado após o toque")]
	public float time;
	[Header("Layers que podem desativar instantaneamente ao colidir")]
	public LayerMask canDestroy;

	private BoxCollider2D platCol;
	private bool disable, disableInst;
	private float timer;

	public GameObject platChild;
	// Use this for initialization
	void Start () {
		
		disable = disableInst = false;

	}
	
	// Update is called once per frame
	void Update () {
		
		timer += Time.deltaTime;
		
		if (!platChild.activeSelf && !disable && !disableInst && timer >= tempoOff) {
			timer = 0;
			platChild.SetActive (true);
		}
		
		
		if (desativa) {
			if (timer >= time) {
				desativa = false;
				timer = 0;
				platChild.SendMessage ("disableMe");
			}
		}

		if (disableInst){
			timer = 0;
			desativa = false;
			disableInst = false;
			platChild.SendMessage ("disableMe");
		}
			
	}

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.layer == canDestroy) {
			timer = 0;
			disableInst = true;
		}
		 if (col.gameObject.layer == 15) {
			timer = 0;
			disable = true;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		
		if (canDestroy == (canDestroy | (1 << col.gameObject.layer))) {
			timer = 0;
			disableInst = true;
		}
		if (col.gameObject.layer == 15) {
			timer = 0;
			disable = true;
		}

	}

}