using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class __GCMENU__ : MonoBehaviour {

	[Header("Acesso ao animator controller do menu")]
	public Animator menuAnim;
	[Header("Colocar numero da spawn do lvl")]
	public int saidaLvl;

	private int controll;
	public string nextSceneName;

	void Start (){
		controll = 1;
		menuAnim.SetBool("playselected", true);
		menuAnim.SetBool("optionsselected", false);
		menuAnim.SetBool("quitselected", false);
	}

	void Update (){

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			controll -= 1;
			if (controll <= 0)
				controll = 3;
            SoundEffectScript.Instance.MakeMenuChangeSound();
            //efeito sonoro
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			controll += 1;
			if (controll >= 4)
				controll = 1;
            SoundEffectScript.Instance.MakeMenuChangeSound();
            //efeito sonoro
        }

		if (controll == 1) {
			menuAnim.SetBool("playselected", true);
			menuAnim.SetBool("optionsselected", false);
			menuAnim.SetBool("quitselected", false);
		}
		if (controll == 2) {
			menuAnim.SetBool("playselected", false);
			menuAnim.SetBool("optionsselected", true);
			menuAnim.SetBool("quitselected", false);
		}
		if (controll == 3) {
			menuAnim.SetBool("playselected", false);
			menuAnim.SetBool("optionsselected", false);
			menuAnim.SetBool("quitselected", true);
		}

		if (Input.GetKeyDown ("return")) {
            if (controll == 1)
                Play();
            SoundEffectScript.Instance.MakeSelectionSound();
            
            if (controll == 2)
               Opcoes ();
            SoundEffectScript.Instance.MakeSelectionSound();
            if (controll == 3)
              Sair ();
            SoundEffectScript.Instance.MakeSelectionSound();
        }

	}

	void Play(){
		Global.saida = saidaLvl;
		Loading.cenaACarregar = nextSceneName;
		Loading.load = true;
	}

	void Opcoes(){
        Loading.cenaACarregar = "Creditos";
        Loading.load = true;
    }

	void Sair(){
		Application.Quit ();
	}





}
