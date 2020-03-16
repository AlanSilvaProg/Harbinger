using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour {

	[Header("Acesso ao transform do ponto de disparo")]
	public Transform pointD;
	public Transform pointDRef;
	public Transform pointERef;
	[Header("Controle de Burst nos tipos de disparo")]
	public bool[] isBurst; // se é burst ou não a arma
	[Header("Acesso aos Pre Fabs das balas")]
	public GameObject[] bullet; // referencia do prefab de cada tipo de bala
	[Header("Controle de Burst")]
	public float burstPause; // pausa entre cada tiro em burst
	public float burstVelo; // taxa de disparo
	public int BurstQuant; // quantidade de tiros por cada burst
	[Space(5)]
	public int equippedGun; // info numérico da arma equipada
	[Header("Tempo entre cada tiro de Lança Granadas")]
	public float grenadeTime; 
	[Header("Tempo permitido para explodir ao realizar novo disparo")]
	public float timeToExplode;
	[Header("Municão inicial para cada arma")]
	public int[] initialMunition;
	private bool dir, esq; // direções dos tiros 
	private bool[] inBurst; //se está em execução algum burst fire
	private float grenadeTimer;
	private GameObject pointDGO;
    bool shooting;

	public float timePerShot;
	private float cooldownTimer;
	public GameObject MultiShotDisplay;
	public GameObject JammerDisplay;
	public GameObject GrenadeDisplay;
	public GameObject AmmoCounter;
	public GameObject AmmoCounterJammer;
	public GameObject AmmoCounterGrenade;

	public GameObject AmmoCounterSymbol;
	public GameObject AmmoCounterSymbolJammer;
	public GameObject AmmoCounterSymbolGrenade;


	private float timehelddown;
	private float timeSinceLastShot;

    public Animator anim;

	private Player playerScript;

	private Text multishotText;
	private Text jammerText;
	private Text grenadeText;
	private Image AmmoCounterImg;
	private Image AmmoCounterImgJammer;
	private Image AmmoCounterImgGrenade;

    void Start(){
		multishotText = AmmoCounter.GetComponent<Text> ();
		jammerText = AmmoCounterJammer.GetComponent<Text> ();
		grenadeText = AmmoCounterGrenade.GetComponent<Text> ();

		AmmoCounterImg = AmmoCounterSymbol.GetComponent<Image> ();
		AmmoCounterImgJammer = AmmoCounterSymbolJammer.GetComponent<Image> ();
		AmmoCounterImgGrenade = AmmoCounterSymbolGrenade.GetComponent<Image> ();

        // iniciando variáveis necessárias para manter controle sobre informações de cada arma
        shooting = false;
        inBurst = new bool[3];

		for (int i = 0; i < 3; i++) {
			inBurst [i] = false;
		}

	}

	// Update is called once per frame
	void Update () {

		if (playerScript == null) {
			playerScript = Global.PlayerAcess;
		}
        
        anim.SetBool("shootOn", shooting);
        anim.SetBool("NoAmmo", Global.ammo[equippedGun] <= 0);

//Atualizando Ponto de disparo

		switch (Global.lastLook) {
		case "dir":
			pointD.position = new Vector2 (pointDRef.position.x, pointDRef.position.y);
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
			break;
		case "esq":
			pointD.position = new Vector2 (pointERef.position.x, pointERef.position.y);
			if (pointD.localRotation.z != 180) {
				transform.rotation = Quaternion.Euler (0.0f, 0.0f, 180.0f);
			}
			break;
		}

		if (Input.GetAxisRaw ("Vertical") > 0 && pointD.localRotation.z != 90) {
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, 90.0f);
		}

		if (Input.GetAxisRaw ("Vertical") < 0 && pointD.localRotation.z != 270) {
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, 270f);
		}


//Timer para explodir granada da cena ao atirar novamente
        if (Global.grenadeOnScene)
			grenadeTimer += Time.deltaTime;

//acionar direções e nega-las
		if (Input.GetAxisRaw ("Horizontal") > 0){dir = true; esq = false;}     

		if (Input.GetAxisRaw ("Horizontal") < 0){dir = false; esq = true;}     

		if (Input.GetAxisRaw ("Horizontal") == 0) {dir = false; esq = false;}

        //Trocando de arma
        if (Input.GetKeyDown("w") && !Global.pause)
        {
            equippedGun = 0;
            anim.SetBool("JammerEqp", false);
            anim.SetBool("GranadeEqp", false);
			JammerDisplay.SetActive (false);
			MultiShotDisplay.SetActive (true);
			GrenadeDisplay.SetActive (false);
        }
		if (Input.GetKeyDown("e") && !Global.pause && Global.jammer || Input.GetKeyDown("e") && !Global.pause && playerScript.enabled)
        {
            equippedGun = 1;
            anim.SetBool("JammerEqp", true);
            anim.SetBool("GranadeEqp", false);
			JammerDisplay.SetActive (true);
			MultiShotDisplay.SetActive (false);
			GrenadeDisplay.SetActive (false);
        }
		if (Input.GetKeyDown("r") && !Global.pause && Global.granada || Input.GetKeyDown("r") && !Global.pause && playerScript.enabled)
        {
            equippedGun = 2;
            anim.SetBool("GranadeEqp", true);
            anim.SetBool("JammerEqp", false);
			JammerDisplay.SetActive (false);
			MultiShotDisplay.SetActive (false);
			GrenadeDisplay.SetActive (true);
        }

//Atirando
		if(cooldownTimer <= 0){
			if (Input.GetKeyDown ("d") && !Global.pause && !Global.recharging && !Global.onDash && !Global.isKnock && !Global.onStasi && !Global.dead && !Global.onScan) {
				shooting = true;
				cooldownTimer = timePerShot;
				timehelddown = 0;
            
				if (Global.lastLook == "esq" && !(Input.GetAxisRaw ("Vertical") != 0) || Global.lastLook == "dir" && !(Input.GetAxisRaw ("Vertical") != 0)) {
                    
					anim.SetTrigger ("shootFrontOn");
					StartCoroutine (shootingwaiter ());
				}

				if (Input.GetAxisRaw ("Vertical") > 0) {
                    
					anim.SetTrigger ("shootUpOn");
					StartCoroutine (shootingwaiter ());
				}

				if (Input.GetAxisRaw ("Vertical") < 0) {
                    
					anim.SetTrigger ("shootDownOn");
					StartCoroutine (shootingwaiter ());
				}
            

				int equip = equippedGun;
				if (isBurst [equip] && !inBurst [equip]) {
					if (Global.ammo[equip] >= 0)
						StartCoroutine ("BurstFire");
				} else if (!isBurst [equip] && Global.ammo[equip] >= 0) {
					if (equip == 2 && !Global.grenadeOnScene && Global.ammo[equip] > 0) {
						if (Global.lastLook == "dir") {
							Instantiate (bullet [equip], new Vector2 (pointD.position.x + bullet [equip].transform.localScale.x / 2, pointD.position.y), pointD.rotation);
							timeSinceLastShot = 0;

						} else if (Global.lastLook == "esq") {
                        
							Instantiate (bullet [equip], new Vector2 (pointD.position.x - bullet [equip].transform.localScale.x / 2, pointD.position.y), pointD.rotation);
							timeSinceLastShot = 0;

						} else {
							Instantiate (bullet [equip], new Vector2 (pointD.position.x, pointD.position.y), pointD.rotation);
							timeSinceLastShot = 0;
						}
                        Global.ammo[equip] -= 1;
						grenadeTimer = 0;
					} else {
						if (Global.grenadeOnScene && grenadeTimer >= timeToExplode) {
							GameObject.FindGameObjectWithTag ("Granada").SendMessage ("PlayDeathParticle");
							Global.balasNaCena [2] -= 1;
							Global.grenadeOnScene = false;
						} else if( Global.ammo[equip] > 0) {
							if (Global.lastLook == "dir") {
								Instantiate (bullet [equip], new Vector2 (pointD.position.x + bullet [equip].transform.localScale.x / 2, pointD.position.y), pointD.rotation);
								timeSinceLastShot = 0;
							} else if (Global.lastLook == "esq") {
								Instantiate (bullet [equip], new Vector2 (pointD.position.x - bullet [equip].transform.localScale.x / 2, pointD.position.y), pointD.rotation);
								timeSinceLastShot = 0;

							} else {
								Instantiate (bullet [equip], new Vector2 (pointD.position.x, pointD.position.y), pointD.rotation);
								timeSinceLastShot = 0;

							}
							Global.ammo[equip] -= 1;
						
						}
					}	
				}
	
			}


			if (Input.GetKey ("d")) {
				timehelddown += 1 * Time.deltaTime;
				if (timehelddown >= 1f) {
					if (equippedGun == 0) {
						StartCoroutine ("BurstFire");
						timehelddown = 0;
					}
				}

			}

			if (timeSinceLastShot >= 2) {
				InitialAmmo (0);
			}
			if (timeSinceLastShot >= 4) {
				InitialAmmo (1);
			}

			if (timeSinceLastShot >= 8) {
				InitialAmmo (2);
			}

		}
		cooldownTimer -= 1 * Time.deltaTime;
		multishotText.text= Global.ammo[0].ToString ();
		jammerText.text = Global.ammo[1].ToString ();
		grenadeText.text = Global.ammo [2].ToString ();
		timeSinceLastShot += 1 * Time.deltaTime;

		AmmoCounterImg.fillAmount = timeSinceLastShot / 2;
		AmmoCounterImgJammer.fillAmount = timeSinceLastShot / 4;
		AmmoCounterImgGrenade.fillAmount = timeSinceLastShot / 8;
	}

	public void InitialAmmo(int i){
        

			Global.ammo[i] = initialMunition [i];

            
      

	}


    IEnumerator shootingwaiter()
    {
        
        
        yield return new WaitForSeconds(3);
        shooting = false;
        
    }


    //Executando um BurstFire
    IEnumerator BurstFire(){
		
		int equip = equippedGun; // guardando atual arma equipada
		inBurst [equip] = true; // ativando controle de burst em execução

		for (int i = 0; i < BurstQuant; i++) {
			timeSinceLastShot = 0;
			if (Global.lastLook == "esq" && !(Input.GetAxisRaw ("Vertical") != 0) || Global.lastLook == "dir" && !(Input.GetAxisRaw ("Vertical") != 0)) {

				anim.SetTrigger ("shootFrontOn");
				StartCoroutine (shootingwaiter ());
			}

			if (Input.GetAxisRaw ("Vertical") > 0) {

				anim.SetTrigger ("shootUpOn");
				StartCoroutine (shootingwaiter ());
			}

			if (Input.GetAxisRaw ("Vertical") < 0) {

				anim.SetTrigger ("shootDownOn");
				StartCoroutine (shootingwaiter ());
			}
			if (Global.ammo[equip] > 0) {
				
                Instantiate (bullet [equip], new Vector2(pointD.position.x + bullet[equip].transform.localScale.x/2, pointD.position.y), pointD.rotation);
				Global.ammo[equip] -= 1;
				//Tocar aqui o som de disparo tendo em mente a Arma Equipada, crie um array de 2 sons, 0 = arma basica, 1 = jammer

				yield return new WaitForSeconds (burstVelo);
			} else {
				i = BurstQuant - 1;


				//Tocar Som de tentativa de tiro sem munição aqui
			}
		}
		yield return new WaitForSeconds (burstPause);
		inBurst [equip] = false; // negando burst em execução
	}
}