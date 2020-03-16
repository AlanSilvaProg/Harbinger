using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esconder : MonoBehaviour {

	private Transform playerObj, GCObj, CameraObj;  

	void Start () {

		if (Global.esconderTestes && !Global.primeiroSum)
			this.gameObject.SetActive(false);

		playerObj = (FindObjectOfType (typeof(Player)) as Player).transform;
		GCObj = (FindObjectOfType (typeof(__GC__)) as __GC__).transform;
		CameraObj = (FindObjectOfType (typeof(InverteLado)) as InverteLado).transform;



	

	}
	void Update () {

		if (Global.primeiroSum && Global.esconderTestes) {
			playerObj.SetParent (null);
			GCObj.SetParent (null);
			CameraObj.SetParent (null);
			Global.primeiroSum = false;
		}

		if (Global.esconderTestes)
			this.gameObject.SetActive(false);

	}
}
