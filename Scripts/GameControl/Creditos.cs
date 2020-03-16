using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creditos : MonoBehaviour {

	public float velo;
	public float tempoCred;
	public RectTransform fim;
	private bool timerCount;
	private float timer;
	
	// Update is called once per frame
	void Update () {

		print (GetComponent<RectTransform> ().localPosition);
		print (fim.localPosition);

		if (GetComponent<RectTransform>().localPosition.y - fim.localPosition.y >= 1) {
			if (timer < tempoCred) {
				timer += Time.deltaTime;
			} else {
				SceneManager.LoadScene ("MainMenu");
			}
		} else {
			transform.Translate (0, velo * Time.deltaTime, 0);
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "fimCred") {
			print ("foi");
			timerCount = true;
		}
	}
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "fimCred") {
			print ("foi");
			timerCount = true;
		}
	}

}
