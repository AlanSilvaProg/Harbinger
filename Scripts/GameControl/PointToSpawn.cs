using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToSpawn : MonoBehaviour {
	[Range(0,10)]
	public int numeroSaida;

	private Transform playerTransf;
	private Player playerScript;
	private Vector3 positionToSpawn;


	// Use this for initialization
	void Start () {

		positionToSpawn = transform.position;
		spawnar ();

	}
	
	// Update is called once per frame
	void Update () {



	}

	void spawnar(){
		if (Global.saida == numeroSaida) {
			playerScript = FindObjectOfType (typeof(Player)) as Player;
			positionToSpawn = gameObject.transform.position;
			playerTransf = playerScript.transform;
			print (positionToSpawn);
			playerTransf.SetPositionAndRotation(positionToSpawn,playerTransf.rotation);
		}
	}
}
