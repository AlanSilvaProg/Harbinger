using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miraCerb : MonoBehaviour {

	private Vector3 alvo; // Alvo que a mira irá se basear para manter sua mira nele

	void Update () {

		alvo = Global.alvo; // atualizando posição do alvo
		OlharAlvo (); // chamando função de atualização de rotação para a direção do alvo


	}

//função de atualização de rotação para a direção do alvo
	void OlharAlvo ()
	{


		Vector3 difference = alvo - transform.position;
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);


	}

}
