using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secrets : MonoBehaviour {

	[Header("Invisibilidade que terá ao ter o player sobre ele")]
	[Range(0,1)]
	public float invisibility;

	private SpriteRenderer sprite;
	private bool above;
	private Color newColor;

	void Start(){

		sprite = GetComponent<SpriteRenderer> ();

	}

	void Update(){

		if (above) {
			if (sprite.color.a > invisibility) {
				newColor = sprite.color;
				newColor.a -= 0.1f;
				sprite.color = newColor;
			}
		} else if (!above) {
			if (sprite.color.a < 1) {
				newColor = sprite.color;
				newColor.a += 0.1f;
				sprite.color = newColor;
			}
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			above = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			above = false;
		}
	}


}
