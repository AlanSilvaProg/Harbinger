using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine.Timeline;

public class InverteLado : MonoBehaviour {


	public GameObject[] Cinemachines;

	private Transform playerT;
	private float counter = 0;
	private bool change = false;
    public float distance = 9;

	// Use this for initialization
	void Start (){


	}
	
	// Update is called once per frame
	void Update () {

		playerT = GameObject.FindGameObjectWithTag ("Player").transform;

		float distanceX;
		distanceX = Mathf.Abs(playerT.position.x - transform.position.x);

		if (counter >= 3)
			change = false;

		if (change == true)
			counter += Time.deltaTime;


		if (distanceX >= distance && Cinemachines [0].activeInHierarchy && change == false) {
			counter = 0;
			change = true;
			Cinemachines [0].SetActive (false);
			Cinemachines [1].SetActive (true);
		} else if (distanceX >= distance && Cinemachines [1].activeInHierarchy && change == false) {
			counter = 0;
			change = true;
			Cinemachines [0].SetActive (true);
			Cinemachines [1].SetActive (false);
		}

	}
}
