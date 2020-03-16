using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxBalaCerb : MonoBehaviour {

	private Vector3 alvo;


	void Start () {
		OlharAlvo();
	}
	

	void Update () {
		alvo = Global.alvo;
	}

	void OlharAlvo ()
	{
		Vector3 difference = alvo - transform.position;
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);

	}

	IEnumerator autoDestruir(){
		yield return new WaitForSeconds (1);
		Destroy (gameObject);
	}

}
