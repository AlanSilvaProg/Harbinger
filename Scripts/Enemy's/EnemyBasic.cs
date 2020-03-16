using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour {

	public Transform player;
	public float vida, municao;
	public bool perseguir, atirar, pular, andar;

	// Update is called once per frame
	void Update () {
		
	}

	public void atingidoBala(float dano){
		print("ai aiii aiii" + dano);
	}
	public void atingidoJammer(float dano){
		print("ai aiii aiii jammer" + dano);
	}

}
