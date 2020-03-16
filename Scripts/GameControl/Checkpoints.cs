using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour {

	public Rigidbody2D CheckRB;
	private bool podeSalvar = true;
	private float lifeToSave, energiaToSave;
	private int armaEquipadaToSave;
	private int[] municaoToSave = new int[3];
	private string ultimoOlharToSave;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


	}
	void OnTriggerEnter2D(Collider2D col){
		if (podeSalvar == true && col.gameObject.tag == "Player") {
			lifeToSave = Global.life;
			energiaToSave = Global.Energia;
			armaEquipadaToSave = Global.armaEquipada;
			for (int i = 0; i < municaoToSave.Length; i++)
				municaoToSave [i] = Global.municao [i];
			ultimoOlharToSave = Global.ultimoOlhar;
			Save ();
			podeSalvar = false;
		}
	}

	public void Save(){
		PlayerPrefs.SetFloat ("Life", lifeToSave);
		PlayerPrefs.SetFloat ("Energia", energiaToSave);
		PlayerPrefs.SetInt ("Equipado", armaEquipadaToSave);
		PlayerPrefs.SetInt ("Municao0", municaoToSave[0]);
		PlayerPrefs.SetInt ("Municao1", municaoToSave[1]);
		PlayerPrefs.SetInt ("Municao2", municaoToSave[2]);
		PlayerPrefs.SetString ("UltimoOlhar", ultimoOlharToSave);
		Global.lastPositionToLoad = CheckRB.position;
		PlayerPrefs.SetString ("Cena", SceneManager.GetActiveScene ().name);
	}
}