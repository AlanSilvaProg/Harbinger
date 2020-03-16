using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLvls : MonoBehaviour {
	[Header("Escreva o nome da cena que carregará")]
	[Header("quando chegar nesse ponto")]
	public string nextSceneName; // próxima cena que será carregada
	[Header("Numero do ponto de saida no próximo lvl")]
	public int saida;

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
			col.gameObject.GetComponent<Collider2D> ().enabled = false;
			col.gameObject.transform.SetParent (null);
			DontDestroyOnLoad (col.gameObject);
			Global.randomValue = (int)Random.Range (1, 7);
			Global.saida = saida;

			if (nextSceneName == "R0" || nextSceneName == "R1" || nextSceneName == "R2" || nextSceneName == "R3" || nextSceneName == "R4") {
				Global.Tema = 1;
			}
			if (nextSceneName == "L0" || nextSceneName == "L1" || nextSceneName == "L2" || nextSceneName == "L3" || nextSceneName == "L4") {
				Global.Tema = 1;
			}
			if (nextSceneName == "L6" || nextSceneName == "L7" || nextSceneName == "L8" || nextSceneName == "L9" || nextSceneName == "L10"
			|| nextSceneName == "L11" || nextSceneName == "L12" || nextSceneName == "L13" || nextSceneName == "L14" || nextSceneName == "L15"
			|| nextSceneName == "L16") {
				Global.Tema = 3;
			}
			if (nextSceneName == "R6" || nextSceneName == "R7" || nextSceneName == "R8" || nextSceneName == "R9" || nextSceneName == "R10"
			|| nextSceneName == "R11" || nextSceneName == "R12" || nextSceneName == "R13" || nextSceneName == "R14" || nextSceneName == "R15"
			|| nextSceneName == "R16" || nextSceneName == "R17") {
				Global.Tema = 3;
			}

			if (nextSceneName == "CERBERUS_STAGE") {
				Global.Tema = 5;
			}

			if (nextSceneName == "APOLO") {
				Global.Tema = 6;
			}

			if (nextSceneName == "T2 R0" || nextSceneName == "T2 R1" || nextSceneName == "T2 R2" || nextSceneName == "T2 R3") {
				Global.Tema = 2;
			}

			if (nextSceneName == "T2 L0" || nextSceneName == "T2 L1" || nextSceneName == "T2 L2" || nextSceneName == "T2 L3"
			|| nextSceneName == "T2 L4") {
				Global.Tema = 2;
			}

			if (nextSceneName == "T2 L5" || nextSceneName == "T2 R4") {
				Global.Tema = 4;
			}

			Loading.cenaACarregar = nextSceneName;
			Loading.load = true;
		}
	}

}
