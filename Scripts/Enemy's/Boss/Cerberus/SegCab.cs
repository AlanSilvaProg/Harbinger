using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegCab : MonoBehaviour {

	public float vida;
	public bool Cabeca_Direita;
	public bool Cabeca_Esquerda;

	public bool sobreEfeitoJammer;
	public float tempoImune;
	public float tempoEfeitoJammer;
	private float timerTravado;

//Comportamentos principais

	public bool despertando = false;
	public bool travado = false;
	private bool on = false;
	public bool caminhando = false;
	public bool selecionando_destino = false;
	public bool selecionando_atq = false;
	public bool preparando_atq = false;
	public bool on_charge = false;
	public bool atacando_ram = false;
	public bool atacando_beam = false;
	public bool atacando_destroy = false;
	public bool die = false;

//Controle de Comportamentos secundarios

	public bool pode_mover = true;
	public bool pode_atacar = false;

	private bool despertado = false;
	public bool chegou = false;
	public bool selecionou = false;
	public bool carregou = false;
	public bool ataqueFim = false;

//Acessos necessários adicionais

	public Vector3 alvo;
	private Rigidbody2D cerbRB;

//Controles de comportamentos secundarios

	public bool fimAndamento;

	public float tempoDespertar;
	private float contTimerDespertar;
	private Vector2 destinoMov;
	public int indiceAtq = 0;

	public Transform[] pontoD;
	public GameObject balaCerb;
	public GameObject balaSegue;
	public int randomShots;
	public bool atirando;
	public float pausaTiros;
	public float pausaTirosS;
	private Vector2 posDestroy;
	public bool podeAtirar;

	public GameObject laserBeam;
	public float tempoLaserAtivo;
	public float tempoLaserAtivar;
	public bool laserAtivo;
	private float timerBeam;
	private Vector2 destinoBeam;
	public float veloBeamAtq;

	private bool preparoAndamento;
	private Vector2 posInicialRam;
	private Vector2 destinoRam;
	public float veloRamAtq;

	public float tempoCharge;
	public float veloPadrao;

	public float tempoPosAtq;

	private bool isChegouRoutineRunning = false;
    private Player playerScript;
    public AudioClip shootsound;
    public float shootVolume = .2f;


    public Animator cerbAnims;

	private float rotationZ;
	void Start () {

		cerbRB = GetComponent<Rigidbody2D> ();
		//laserBeam.SetActive(false);

	}

	void Update () {
		//print (atacando_beam);
		alvo = Global.alvo;

		timerTravado += Time.deltaTime;
		if (!travado && on && despertado && !Global.pause) {
		////////////////////////////////////////////////////
			 
			if (ataqueFim && !fimAndamento) {
				fimAndamento = true;
				//StopCoroutine ("ReiniciarCiclo");
				//StartCoroutine ("ReiniciarCiclo");
				ReiniciarCiclo();
				ReiniciarCiclo ();
			}

			if (laserAtivo && timerBeam < tempoLaserAtivo) {
				timerBeam += Time.deltaTime;
			} else if (laserAtivo && timerBeam > tempoLaserAtivo) {
				timerBeam = 0;
				//laserBeam.SetActive (false);
				ataqueFim = true;
				atacando_ram = false;
				atacando_destroy = false;
				atacando_beam = false;
				pode_atacar = false;
				laserAtivo = false;
				
			}

			if (preparoAndamento && indiceAtq == 1 && !laserAtivo || atacando_beam && !laserAtivo) {
				transform.position = Vector2.MoveTowards (transform.position, destinoBeam, veloBeamAtq * Time.deltaTime);
			} else if (preparoAndamento && indiceAtq == 1 && laserAtivo || atacando_beam && laserAtivo) {
				transform.position = Vector2.MoveTowards (transform.position, destinoBeam, veloBeamAtq/2 * Time.deltaTime);
			}

			if (!preparando_atq && !preparoAndamento && !on_charge && pode_atacar && !ataqueFim) {
				if (atacando_ram) {
					AtaqueRam ();
				}
				if (atacando_beam) {
					AtaqueBeam ();
				}
				if (atacando_destroy && !atirando) {
					AtaqueDestroy ();
				}
			}

			if (preparando_atq && preparoAndamento && !on_charge) {
				if (indiceAtq == 0) {
					if (preparoAndamento && Vector2.Distance (transform.position, posInicialRam) > 1) {
						PreparoFinalRam ();
					}else if(preparoAndamento && indiceAtq == 0 && Vector2.Distance (transform.position, posInicialRam) <= 1){
						StopCoroutine ("OnCharge");
						StartCoroutine ("OnCharge");
					}
				}
				if (indiceAtq == 1) {					
					StopCoroutine ("OnCharge");
					StartCoroutine ("OnCharge");
				}
				if (indiceAtq == 2) {
					if (podeAtirar) {
						StopCoroutine ("OnCharge");
						StartCoroutine ("OnCharge");
					} else {
						PreparoFinalDestroy ();
					}
				}
			}

			if (preparando_atq && !on_charge && !pode_atacar && !chegou && !preparoAndamento) {
				if (indiceAtq == 0) {
					PrepararRam ();
				}
				if (indiceAtq == 1) {
					PrepararBeam ();
				}
				if (indiceAtq == 2) {
					PrepararDestroy ();
				}
			}
				
			if (!selecionando_atq && selecionou && !pode_mover && !pode_atacar) {
				preparando_atq = true;
				selecionou = false;
			}

			if (selecionando_atq && !selecionou && !pode_mover && !pode_atacar) {
				ataqueFim = false;
				SelecionandoAtk ();
			} else if (selecionou) {
				selecionando_atq = false;
			}

			if (caminhando && selecionou && !chegou && pode_mover && !pode_atacar) {

				Caminhar ();

			} else if (caminhando && selecionou && chegou && pode_mover && !pode_atacar) {
				selecionando_atq = true;
				caminhando = false;
				selecionou = false;
				pode_mover = false;
				chegou = false;

			}
				
			if (pode_mover && selecionando_destino && !selecionou && !pode_atacar) {
				SelecionarDest ();
			} else if (selecionando_destino && selecionou) {  selecionando_destino = false;  }


			if (!caminhando && pode_mover && !pode_atacar) {  selecionando_destino = true;  caminhando = true;}



		///////////////////////////////////////////////////////////////////////////////////
		} else {
			if (despertando && contTimerDespertar < tempoDespertar && GlobalCerberus.onPrincipal) {
				Despertar();
			} 
			else if (despertando && contTimerDespertar >= tempoDespertar) {
				despertando = false;
				despertado = true;
				contTimerDespertar = 0;
			}

			if (!despertado && !despertando) {	
				despertando = true;
			}

			if (travado) {  cerbRB.velocity = Vector2.zero;  }

			if (travado && timerTravado > tempoEfeitoJammer) {
				travado = false;
			}

		}
		cerbAnims.SetBool ("HeadButtOn", atacando_ram);
		cerbAnims.SetBool ("LaserOn", atacando_beam);
		cerbAnims.SetBool ("MissileOn", atacando_destroy);
		if(laserBeam.activeSelf){
			OlharAlvoLazor ();
		} else if (!atacando_ram) {
			OlharAlvo ();
		}

	}

	void Despertar(){
		on = true;
		contTimerDespertar += Time.deltaTime;


	}

    void SelecionarDest(){
		float distancePCX; // distancia entre camera e o player do lado direito ou esquerdo
		float distancePCY;
		float randomPositionX = 0;
		float randomPositionY = 0;

		if (Cabeca_Direita) {
			
			distancePCX = Mathf.Abs (alvo.x - GlobalCerberus.limitCerb.position.x + GlobalCerberus.limitCerb.localScale.x/2);
			distancePCY = Mathf.Abs (alvo.y - GlobalCerberus.limitCerb.position.y + GlobalCerberus.limitCerb.localScale.y/2);

			randomPositionX = Random.Range (GlobalCerberus.limitCerb.position.x + (distancePCX/100)*25 , (GlobalCerberus.limitCerb.position.x + GlobalCerberus.limitCerb.localScale.x / 2) - transform.localScale.x / 2);
			//randomPositionX = Random.Range (Global.PlayerAcess.transform.position.x , (Global.PlayerAcess.transform.position.x + (distancePCX/100)*25 ) - transform.localScale.x / 2);
			if (alvo.x > GlobalCerberus.limitCerb.position.x + GlobalCerberus.limitCerb.localScale.x/4) {
				
				randomPositionX = Random.Range (alvo.x - (GlobalCerberus.limitCerb.position.x - GlobalCerberus.limitCerb.localScale.x / 6), GlobalCerberus.limitCerb.position.x);
			}

			if ((int)Random.Range (1, 3) == 1) {
				randomPositionY = Random.Range (alvo.y + 4, (GlobalCerberus.limitCerb.position.y + GlobalCerberus.limitCerb.localScale.y / 2) - transform.localScale.y / 2);
			} else {
				randomPositionY = Random.Range (alvo.y - 4, (GlobalCerberus.limitCerb.position.y - GlobalCerberus.limitCerb.localScale.y / 2) + transform.localScale.y / 2);
			}

		} else if (Cabeca_Esquerda) {
			
			distancePCX = Mathf.Abs (alvo.x - GlobalCerberus.limitCerb.position.x - GlobalCerberus.limitCerb.localScale.x/2);
			distancePCY = Mathf.Abs (alvo.y - GlobalCerberus.limitCerb.position.y + GlobalCerberus.limitCerb.localScale.y/2);

			randomPositionX = Random.Range (GlobalCerberus.limitCerb.position.x - (distancePCX/100)*25 , (GlobalCerberus.limitCerb.position.x - GlobalCerberus.limitCerb.localScale.x / 2) + transform.localScale.x / 2);

			if (alvo.x < GlobalCerberus.limitCerb.position.x - GlobalCerberus.limitCerb.localScale.x/4) {

				randomPositionX = Random.Range (alvo.x + (GlobalCerberus.limitCerb.position.x + GlobalCerberus.limitCerb.localScale.x / 6), GlobalCerberus.limitCerb.position.x);
			}

			if ((int)Random.Range (1, 3) == 1) {
				randomPositionY = Random.Range (alvo.y + 4, (GlobalCerberus.limitCerb.position.y + GlobalCerberus.limitCerb.localScale.y / 2) - transform.localScale.y / 2);
			} else {
				randomPositionY = Random.Range (alvo.y - 4, (GlobalCerberus.limitCerb.position.y - GlobalCerberus.limitCerb.localScale.y / 2) + transform.localScale.y / 2);
			}

		}

		destinoMov = new Vector2 (randomPositionX, randomPositionY);
		selecionou = true;
		chegou = false;

	}

	void Caminhar(){

		if (Vector2.Distance(new Vector2(Mathf.Abs(transform.position.x),Mathf.Abs(transform.position.y)), new Vector2(Mathf.Abs(destinoMov.x),Mathf.Abs(destinoMov.y))) > 1) {
			transform.position = Vector2.MoveTowards (transform.position, destinoMov, veloPadrao * Time.deltaTime);
		} else {
			//chegou = true;
			if (isChegouRoutineRunning == false) {
				StartCoroutine ("ChegarEmDestino");
			}
		}
		
	}

	void SelecionandoAtk(){
		//print ("SELECIONANDOATK");
		//print (GlobalCerberus.podeDestroy + " Destroy");
	//	print (GlobalCerberus.podeRam  +" RAM");
	//	print (GlobalCerberus.podeBeam + " BEAM");
		//print ("SELECIONADOATK");
		indiceAtq = Random.Range (0, 3);
		//rint (indiceAtq);
		switch (indiceAtq) {
		case 0:
			if (GlobalCerberus.podeRam) {
				GlobalCerberus.podeRam = false;
				selecionou = true;
			} else {
				indiceAtq += 1;
			}
			break;
		case 1:
			if (GlobalCerberus.podeBeam) {
				GlobalCerberus.podeBeam = false;
				selecionou = true;
			}else {
				indiceAtq += 1;
			}
			break;
		case 2:
			if (GlobalCerberus.podeDestroy) {
				GlobalCerberus.podeDestroy = false;
				selecionou = true;
			}else {
				indiceAtq = 0;
				SelecionandoAtk ();
			}
		


			break;
		}
	//	print (GlobalCerberus.podeDestroy + " Destroy");
		//print (GlobalCerberus.podeRam  +" RAM");
		//print (GlobalCerberus.podeBeam + " BEAM");

	}

	void PrepararRam (){

		if (Cabeca_Direita) {
			posInicialRam = new Vector2 (GlobalCerberus.limitCerb.position.x + (GlobalCerberus.limitCerb.localScale.x / 2 - transform.localScale.y / 2), alvo.y);
			destinoRam = new Vector2 (GlobalCerberus.limitCerb.position.x - (30), posInicialRam.y);
		} else {
			posInicialRam = new Vector2 (GlobalCerberus.limitCerb.position.x - (GlobalCerberus.limitCerb.localScale.x / 2 + transform.localScale.y / 2), alvo.y);
			destinoRam = new Vector2 (GlobalCerberus.limitCerb.position.x + (30), posInicialRam.y);
		}

			preparoAndamento = true;


	}

	void PreparoFinalRam(){
		
		transform.position = Vector2.MoveTowards (transform.position, posInicialRam, veloPadrao*Time.deltaTime);

	}

	void AtaqueRam(){
		
		if (Vector2.Distance (transform.position, destinoRam) > 1) {
			transform.position = Vector2.MoveTowards (transform.position, destinoRam, veloRamAtq * Time.deltaTime);
		} else {
			ataqueFim = true;
			atacando_ram = false;
			atacando_destroy = false;
			atacando_beam = false;
			pode_atacar = false;
		}
			
	}

	void PrepararBeam (){
		if (Cabeca_Esquerda) {
			destinoBeam = new Vector2 ((alvo.x - 10) , alvo.y);
		} else {
			posDestroy = new Vector2 ((alvo.x + 10)  ,alvo.y);
		}
		//destinoBeam = new Vector2 (alvo.x , alvo.y + transform.localScale.y * 2);

		preparoAndamento = true;


	}

	void AtaqueBeam(){
		
		destinoBeam = new Vector2 (alvo.x , alvo.y + transform.localScale.y * 2);

	}

	void PrepararDestroy(){

		if (Cabeca_Esquerda) {
			posDestroy = new Vector2 ((alvo.x - 10) , alvo.y);
		} else {
			posDestroy = new Vector2 ((alvo.x+ 10)  ,alvo.y);
		}

		randomShots = (int)Random.Range(0,2);
		preparoAndamento = true;

	}

	void PreparoFinalDestroy (){

		if (Vector2.Distance (transform.position, posDestroy) > 1) {
			transform.position = Vector2.MoveTowards (transform.position, posDestroy, veloPadrao * Time.deltaTime);
		} else {
			podeAtirar = true;
		}


	}

	void AtaqueDestroy (){
		
		atirando = true;

		if (randomShots == 0) {
			StartCoroutine ("ShotsSegue");
		} else if (randomShots == 1) {
			StartCoroutine ("ShotsPD");
		}
	}

	public void atingidoBala (float dmgBala)
	{
		////////
		//Espaço para 	FeedBack de Damage
		///////
		vida -= dmgBala;
	}

	public void atingidoJammer (float dmgBala)
	{
		////////
		//Espaço para 	FeedBack de Damage Jammer
		///////
		if (!travado && timerTravado >= tempoImune) {
			travado = true;
			timerTravado = 0;
		}
		vida -= dmgBala;
	}

	IEnumerator ShotsSegue(){
		cerbAnims.SetTrigger ("OnMissileATK");
		yield return new WaitForSeconds (pausaTirosS);
		Instantiate (balaSegue, pontoD[0].position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD[1].position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD[2].position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD[3].position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);

		Instantiate (balaSegue, pontoD[1].position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);

        yield return new WaitForSeconds(2);
		ataqueFim = true;
		atacando_ram = false;
		atacando_destroy = false;
		atacando_beam = false;
		pode_atacar = false;

	}

	IEnumerator ShotsPD(){
		cerbAnims.SetTrigger ("OnMissileATK");
		yield return new WaitForSeconds (pausaTiros);
		Instantiate (balaCerb, pontoD[0].position, pontoD[0].rotation);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTiros);
		Instantiate (balaCerb, pontoD[1].position, pontoD[1].rotation);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTiros);
		Instantiate (balaCerb, pontoD[2].position, pontoD[2].rotation);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);

        yield return new WaitForSeconds(2);
		ataqueFim = true;
		atacando_ram = false;
		atacando_destroy = false;
		atacando_beam = false;
		pode_atacar = false;

	}

	IEnumerator OnCharge(){
		on_charge = true;
		yield return new WaitForSeconds (tempoCharge);
		preparando_atq = false;
		preparoAndamento = false;
		on_charge = false;
		pode_atacar = true;
		if (indiceAtq == 0) {
			atacando_ram = true;
		}
		if (indiceAtq == 1) {
			cerbAnims.SetTrigger ("OnLaserATK");
			atacando_beam = true;
			StartCoroutine ("LaserActive");
		}
		if (indiceAtq == 2) {
			atacando_destroy = true;
		}
	}

	IEnumerator LaserActive(){
		yield return new WaitForSeconds (tempoLaserAtivar);
		laserAtivo = true;
		timerBeam = 0;
		//laserBeam.SetActive(true);
	}

	//IEnumerator ReiniciarCiclo(){
	void ReiniciarCiclo(){
		//yield return new WaitForSeconds (tempoPosAtq);
		atirando = false;
		ataqueFim = false;
		pode_mover = true;
		fimAndamento = false;
		chegou = false;
		if (indiceAtq == 0) {
			GlobalCerberus.podeRam = true;
			//print ("RESETANDO GLOBAL RAM");
		}
		if (indiceAtq == 2) {
			GlobalCerberus.podeDestroy = true;
			//print ("RESETANDO GLOBAL DESTROY");

		}
		if (indiceAtq == 1) {
			GlobalCerberus.podeBeam = true;
		//	print ("RESETANDO GLOBAL BEAM");

		}
	}

	IEnumerator ChegarEmDestino(){
		isChegouRoutineRunning = true;
		yield return new WaitForSeconds (tempoPosAtq);
		chegou = true;
		isChegouRoutineRunning = false;

	}


	void OnDrawGizmos(){
		Gizmos.DrawSphere (destinoRam, 1);
	}

	void OlharAlvo ()
	{


		Vector3 difference = alvo - transform.position;
		 rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);


	}

	void OlharAlvoLazor ()
	{


		Vector3 difference = alvo - transform.position;
		rotationZ = (Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg);
		rotationZ = (Mathf.MoveTowardsAngle(transform.eulerAngles.z,rotationZ,5*Time.deltaTime));
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);
		//print (transform.eulerAngles);

	}
	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.name == "Player"){
			Global.PlayerAcess.SendMessage("atingido", 10);
			//Global.life -= 10;
			} else {
				

			}
		}

	}