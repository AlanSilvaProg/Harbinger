using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCabPrincipal : MonoBehaviour {

	private Vector3 alvo; // alvo que o laser irá basear sua rotação

	void Update () {

//atualizando alvo
		alvo = Global.alvo;

//Quando inativo, sua rotação será Atualizada em direção ao alvo
		if (!GlobalCerberus.laserPrincipal) {
			OlharAlvo (); // chamando função de atualização de rotações
		}


		
	}
//função de atualização de rotações
	void OlharAlvo ()
	{

		Vector3 difference = alvo - transform.position;
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);


	}

}
