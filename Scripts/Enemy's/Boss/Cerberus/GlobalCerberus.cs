using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCerberus : MonoBehaviour {
	[Header("Linkar o Objeto limitCerberus")]
	public Transform limitCerberus; // Objeto pré criado para verificar se o cerberus está na area, um grande collider trigger 
	//Guardando as informações do limit para ter acesso em outros scripts das cabeças
	public static Transform limitCerb;
	//Variavel que guardara informações como, se a cabeça está on ou não
	public static bool onPrincipal;
	//Controle do laser da cabeça principal, necessario para acessar tanto do script da cabeça principal quanto do laser dela
	public static bool laserPrincipal;
	//Vida das 3 cabeças, necessário para saber dentro de uma cabeça quanto de vida tem a outra
	public static float[] life = new float[3];
	//Variaveis de controle para evitar ataques multiplos
	public static bool podeRam = true, podeBeam = true, podeDestroy = true;



	// Use this for initialization
	void Start () {
		//Guardando as informações do limite
		limitCerb = limitCerberus;
		//Guardando as informações da vida
		life[0] = 100;
		life[1] = 100;
		life[2] = 100;

	}
}
