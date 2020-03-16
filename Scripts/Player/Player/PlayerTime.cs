using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTime : MonoBehaviour {

	[Header("Controle de Energia")]
	[Range(0,100)]
	public float rechargeValue; // recarga por cada tempoRecarga
	[Header("Tempo para cada Recarga")]
	public float rechargeTime; // tempo para cada recarga
	[Header("Velocidade ao entrar em modo de Stasi")]
	public float realTimeOnStasis; // tempo real quando estiver em stasi
	[Header("tempo para cada gasto de Energia")]
	public float timeToReduceEnergy; // tempo para cada gasto de energia
	[Header("Valor de Reducão de Energia quando estiver em Stasi")]
	public int stasiEnergyNeeded; // valor de energia que será gasta
	private Player playerScript;
	private Rigidbody2D playerRB;
	private float time = 1; //time vai dizer como o tempo irá funcionar, sendo 0 paralização total e 1 tempo normal
	private bool recharging = false, recharge = false;
	private bool FastForward = false; //inicia falso, fazer a mudança quando habilidade for ativada


	[Header("animações e partículas")]
    public Animator anim;
    private bool noenergy;
    private bool includeChildren = true;
    private bool ischarging;
    // Use this for initialization
    void Start () {
        noenergy = false;
        ischarging = false;
       
        playerScript = FindObjectOfType (typeof(Player)) as Player;


	}
	
	// Update is called once per frame
	void Update () {

//Atualizando o estado de recarregamento
		Global.recharging = recharge;

        //particles
		anim.SetBool("noEnergy", noenergy);
        anim.SetBool("rechargingOn", ischarging);
		if (Global.Energia > 0) {
			noenergy = false;
		} else if (Input.GetKeyDown ("left shift")) {
			noenergy = true;
		} else {
			noenergy = false;
		}

        //particles-
        playerRB = Global.playerRB;
		
//Vamos manipular o tempo :D

			Time.timeScale = time; 

//Vericando se vamos fazer uma recarguinha 

		if (Input.GetKey ("q") && Global.Energia < 100 && FastForward == false && !Global.pause && !Global.morto && !Global.onScan) {
			recharge = true;

        }
		if (Input.GetKey ("q") && Global.Energia >= 100 && !Global.pause && !Global.morto && !Global.onScan) {
			recharge = false;
            ischarging = false;
		}
		if (Input.GetKeyUp("q") && !Global.pause && !Global.morto && !Global.onScan) {
			recharge = false;
            ischarging = false;
            
        }
		if (Input.GetKeyDown("q") && !Global.pause && Global.Energia <= 100 && !Global.morto && !Global.onScan)
        {
            ischarging = true;

        }

		if (recharge == true && recharging == false && !Global.pause && !Global.morto) {
			StartCoroutine ("RecargaEnerg");
		}
		if (Global.morto) {
			recharge = false;
			ischarging = false;
			anim.SetBool("rechargingOn", false);

		}
//verificando o uso do fast Forward
		if (Global.stasi  || playerScript.ativar) {
			if (Input.GetKeyDown ("left shift") && recharge == false && !Global.pause && !Global.morto && !Global.onScan) {
				if (Global.Energia > 0) {
                    anim.SetBool("timejump", true);
					Global.onStasi = true;
                    

                    FastForward = true;
					time = realTimeOnStasis;
					StartCoroutine ("PerdaEnergia");
				}
			}
			if (Input.GetKeyUp ("left shift") || Global.Energia <= 0 && !Global.pause && !Global.morto) {
				FastForward = false;
				time = 1;
                anim.SetBool("timejump", false);
                noenergy = true;
				Global.onStasi = false;
                StopCoroutine ("PerdaEnergia");
			}
            

        }

//Realizando Fast Forward

		if (time > 1 && FastForward == true && !Global.pause) {
			playerRB.simulated = false;
			playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
		} else {
			playerRB.simulated = true;
			playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
			
	}

	IEnumerator RecargaEnerg(){ //recarregando energia
        
        recharging = true;
		playerScript.StopCoroutine("stateATT");
		playerScript.StartCoroutine("stateATT");
		yield return new WaitForSeconds (rechargeTime);
		Global.Energia += rechargeValue;
		recharging = false;

	}
	IEnumerator PerdaEnergia(){
		Global.Energia -= stasiEnergyNeeded;
		yield return new WaitForSeconds (timeToReduceEnergy);
		StartCoroutine ("PerdaEnergia");
	}

}
