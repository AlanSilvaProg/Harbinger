using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Limit : Cinemachine.CinemachineConfiner {

	private GameObject limit;

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {



		if (m_BoundingShape2D == null) {
			limit = GameObject.FindGameObjectWithTag ("LimitCam");
			m_BoundingVolume = limit.GetComponent<BoxCollider> ();
		}
	

	}
}
