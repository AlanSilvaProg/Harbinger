using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScan : MonoBehaviour {


	//[Header("Tempo para o scan adquirir conhecimento no objeto apontado")]
	//public float tempoLore;
	[Header("Linkar Scanner do personagem")]
	public GameObject Scanner;

	private bool scanOn;

	private float timer;

	// Use this for initialization
	void Start () {

		Scanner.SetActive(false);
		scanOn = false;


	}
	
	// Update is called once per frame
	void Update () {

		Global.onScan = scanOn;

		if (!Global.dead && !Global.onDash && !Global.onStasi && !Global.pause && !Global.recharging && Global.PlayerAcess.canJump) {
			if (Input.GetKey ("a") && !scanOn && !Global.dead && !Global.onDash && !Global.onStasi && !Global.pause && !Global.recharging && Global.PlayerAcess.canJump) {
				Scanner.SetActive (true);
				scanOn = true;
			}

			if (Input.GetKeyUp ("a") && scanOn) {
				Scanner.SetActive (false);
				scanOn = false;
			}
		} else {
			Scanner.SetActive (false);
			scanOn = false;
		}

	}

}
