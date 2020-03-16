using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class __GC__ : MonoBehaviour {

	[Header("Acesso de informações da HUD")]
	public Camera PauseC;
	public Camera HUD;
	[Space(10)]
	public Image[] HUDS = new Image[14];
	[Space(10)]
	public Image HUDMap;
	public Image HUDDano;
	public Image HUDEnergyBar, HUDLifeBar;
    public Image HUDEnergyBar2;
    [Space(10)]
	public Vector3 lastPositionToLoad;
	[Header("Variaveis de controle")]
	public float tempoDmgHUD;

	private float corVisible = 255, corInvisible = 0.3f;

	// Use this for initialization
	void Start () {
		

		PauseC.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Delete)) {
			Sair ();
		}

		if (Input.GetKey (KeyCode.RightControl)) {
			if (Input.GetKeyDown ("a")) {
				Global.saida = 0;
				Loading.cenaACarregar = "Hangar"; // vai para o hangar
				Loading.load = true;
			}
			if (Input.GetKeyDown ("s")) {
				Global.saida = 0;
				Loading.cenaACarregar = "R5"; // pula tutorial
				Loading.load = true;
			}
			if (Input.GetKeyDown ("d")) {
				Global.saida = 0;
				Loading.cenaACarregar = "R17"; // pula para o ultimo level da direita no tier1
				Loading.load = true;
			}
			if (Input.GetKeyDown ("f")) {
				Global.saida = 0;
				Loading.cenaACarregar = "L5"; // pula o tutorial do lado esquerdo
				Loading.load = true;
			}
			if (Input.GetKeyDown ("g")) {
				Global.saida = 0;
				Loading.cenaACarregar = "L16"; // pula para o ultimo level da esquerda no tier1
				Loading.load = true;
			}
			if (Input.GetKeyDown ("h")) {
				Global.saida = 0;
				Loading.cenaACarregar = "T2 R0"; // pula para o primeiro level da direita no Tier 2
				Loading.load = true;
			}
			if (Input.GetKeyDown ("j")) {
				Global.saida = 0;
				Loading.cenaACarregar = "T2 R4"; // pula para o ultimo level da direita no Tier 2
				Loading.load = true;
			}
			if (Input.GetKeyDown ("k")) {
				Global.saida = 0;
				Loading.cenaACarregar = "T2 L0"; // pula para o primeiro level da esquerda no Tier 2
				Loading.load = true;
			}
			if (Input.GetKeyDown ("o")) {
				Global.saida = 0;
				Loading.cenaACarregar = "T2 L5"; // pula para o ultimo level da esquerda no Tier 2
				Loading.load = true;
			}
			if (Input.GetKeyDown ("z")) {
				Global.saida = 0;
				Loading.cenaACarregar = "CERBERUS_STAGE"; // pula para o cerberus
				Loading.load = true;
			}
			if (Input.GetKeyDown ("x")) {
				Global.saida = 0;
				Loading.cenaACarregar = "APOLO"; // pula para o APOLO
				Loading.load = true;
			}
		}
//Mantendo o objeto ao fazer load de uma nova cena
		DontDestroyOnLoad (this.gameObject);

		HUDEnergyBar.fillAmount = Global.Energia / 100;
        HUDEnergyBar2.fillAmount = Global.Energia / 100;
        HUDLifeBar.fillAmount = Global.life / 100;

		if (Input.GetKeyDown ("p") && Global.pause) {
			Despause ();
		} else if (Input.GetKeyDown ("p") && !Global.pause) {
			Pause ();
		}

		if (Global.modeState == true || Time.timeScale == 2) {
			Combate ();
		}

		if (!Global.modeState && Time.timeScale == 1) {
			Passive ();
		}
		
		if (Global.damage) {
			StopCoroutine ("Damage");
			StartCoroutine ("Damage");
		}


	}

	public void Combate(){
		
		Color cor = HUDMap.color;
		cor.a = corInvisible;
		HUDMap.color = cor;

		for (int i = 0; i < HUDS.Length; i++) {
				cor = HUDS [i].color;
				cor.a = corVisible;
				HUDS [i].color = cor;
		}

	}

	public void Passive(){
		
		Color cor = HUDMap.color;
		cor.a = corVisible;
		HUDMap.color = cor;

		for (int i = 0; i < HUDS.Length; i++) {
				cor = HUDS [i].color;
				cor.a = corInvisible;
				HUDS [i].color = cor;
		}

	}

	public void Pause(){

		Global.pause = true;
		PauseC.enabled = true;
		HUD.enabled = false;

	}

	public void Despause (){
		
		Global.pause = false;
		PauseC.enabled = false;
		HUD.enabled = true;

	}

	public void GameOver(){
		
	}

	public void Load(){
		string cena = PlayerPrefs.GetString("Cena");
		if (Global.life <= 0) {
			Global.loadMorte = true;
		} else {
			Global.life = 100;
			Global.Energia = 100;
			Global.armaEquipada = PlayerPrefs.GetInt ("Equipado");
			Global.municao [0] = PlayerPrefs.GetInt ("Municao0");
			Global.municao [1] = PlayerPrefs.GetInt ("Municao1");
			Global.municao [2] = PlayerPrefs.GetInt ("Municao2");
			Global.ultimoOlhar = PlayerPrefs.GetString ("UltimoOlhar");
			Global.PlayerAcess.transform.parent = null;
			Global.PlayerAcess.transform.position = Global.lastPositionToLoad;
		}

		Loading.cenaACarregar = cena;
		Loading.load = true;
		

		
	}

	public void Sair(){
		Application.Quit ();
	}

	public void Menu(){
		
		Loading.cenaACarregar = "MainMenu";
		Loading.load = true;
	}

	IEnumerator Damage(){
		Global.damage = false;

		Color cor = HUDDano.color;
		cor.a = corVisible;
		HUDDano.color = cor;
		yield return new WaitForSeconds (tempoDmgHUD);
		cor.a = corInvisible;
		HUDDano.color = cor;
	}

}
