using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincipaCab : MonoBehaviour {
	
	private float vida = 600;

	[Header("Tempo extra no descanso quando atingido pelo jammer")]
	public float tempoAdicionalJammer;
	private float AdicionalJammer;
	private bool sobreEfeitoJammer;

	public GameObject backbar;
	public GameObject lifebar;
	private Image lifebarimg;
	private float vidamaxima;

    //Comportamentos principais

    public bool despertando = false;
	public bool on = false;
	public bool die = false;

	//Controle de Comportamentos secundarios

	public bool pode_atacar = false;

	public bool despertado = false;
	public bool selecionou = false;
	public bool ataqueFim = false;

	//Acessos necessários adicionais

	public Vector3 alvo;

	//Controles de comportamentos secundarios

	public bool fimAndamento;

	public float tempoDespertar;
	private float contTimerDespertar;
	public int indiceAtq = 0;

	public Transform pontoD;
	public GameObject balaCerb;
	public GameObject balaSegue;
	public int randomShots;
	public float pausaTiros;
	public float pausaTirosS;
	public bool podeAtirar;

	public bool on_charge;
	public bool preparoAndamento;

	public GameObject laserBeam;
	public float tempoLaserAtivo;
	public float tempoLaserAtivar;
	public bool laserAtivo;
	public float timerBeam;

	public float tempoCharge;

	public float tempoPosAtq;

	private Player playerScript;

    [Header("Efeitos")]
    public Animator animator;
    public ParticleSystem b00mfx;
    public AudioClip b00msound;
    public float bo00mVolume = .3f;
    public AudioClip shootsound;
    public float shootVolume = .2f;

    void Start () {
        animator = animator.GetComponent<Animator>();
		lifebarimg = lifebar.GetComponent<Image> ();
        laserBeam.SetActive(false);
		pode_atacar = true;
		vidamaxima = vida;
	}

	void Update () {
		if (on) {
			if (!backbar.activeSelf) {
				backbar.SetActive (true);
			}

			lifebarimg.fillAmount = vida / vidamaxima;
		} else {
			backbar.SetActive (false);
		}

		print (vida);
		if (vida <= 0) {
            Instantiate(b00mfx, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(b00msound, playerScript.transform.position, bo00mVolume);
            Loading.load = true;
			Loading.cenaACarregar = "Hub2";
			Global.saida = 0;
            
        }
//Armazenando em qual Quarto de vida o Cerberu está
		if(vida < 0){
			Global.qual_metade_Cerb = 0;
		}

		if (vida >= 0 && vida < 25) {
			Global.qual_metade_Cerb = 1;
		}

		if (vida >= 25 && vida < 50) {
			Global.qual_metade_Cerb = 2;
		}

		if (vida >= 50 && vida < 75) {
			Global.qual_metade_Cerb = 3;
		}

		if (vida >= 75 && vida <= 100) {
			Global.qual_metade_Cerb = 4;
		}

//Acesso ao script do player
		if (playerScript == null) {
			playerScript = Global.PlayerAcess;
		}

		alvo = playerScript.transform.position;

		if ( on && despertado && !die && !Global.pause) {
			////////////////////////////////////////////////////

			if (ataqueFim && !fimAndamento) {
				fimAndamento = true;
				StartCoroutine ("ReiniciarCiclo");
			}

			if (laserAtivo && timerBeam >= tempoLaserAtivo) {
				laserAtivo = false;
				laserBeam.SetActive (false);
				GlobalCerberus.laserPrincipal = false;
				ataqueFim = true;
			}

			if (laserAtivo) {
				timerBeam += Time.deltaTime;
			}

			if (pode_atacar && !selecionou) {
				SelecionarAtk ();
			} else if (pode_atacar && selecionou) {
				if (indiceAtq == 1) {
					pode_atacar = false;
					prepararDestroy ();
				}
				if (indiceAtq == 0) {
					pode_atacar = false;
					prepararLaser ();
				}
			}


			///////////////////////////////////////////////////////////////////////////////////
		} else {

			if (despertando && contTimerDespertar < tempoDespertar) { Despertar ();	} 
			else if (despertando && contTimerDespertar >= tempoDespertar) {
				despertando = false;
				despertado = true;
				GlobalCerberus.onPrincipal = true;
				contTimerDespertar = 0;
			}

			//if (!despertado && !despertando) {	despertando = true;	 }

		}


	}

	void Despertar(){

		on = true;
		contTimerDespertar += Time.deltaTime;

	}

	void SelecionarAtk(){
		indiceAtq = 1;
		selecionou = true;

		}




	void prepararDestroy (){
		preparoAndamento = true;
		on_charge = true;
		podeAtirar = true;
		StartCoroutine ("OnCharge");
	}

	void prepararLaser (){
		preparoAndamento = true;
		on_charge = true;
		podeAtirar = false;
		StartCoroutine ("OnCharge");
	}

	void AtaqueDestroy (){
			StartCoroutine ("ShotsPD");
	}

	public void atingidoBala (float dmgBala)
	{
        ////////
        //Espaço para 	FeedBack de Damage
        ///////
        animator.SetTrigger("OnHit");
		vida -= dmgBala;
	}

	public void atingidoJammer (float dmgBala)
	{
		////////
		//Espaço para 	FeedBack de Damage Jammer
		///////
		if (!sobreEfeitoJammer) {
			sobreEfeitoJammer = true;
			AdicionalJammer = tempoAdicionalJammer;
		}
		vida -= dmgBala;
	}

	IEnumerator ShotsSegue(){
		yield return new WaitForSeconds (pausaTirosS);
		Instantiate (balaSegue, pontoD.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        yield return new WaitForSeconds(pausaTirosS);
		Instantiate (balaSegue, pontoD.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);

        yield return new WaitForSeconds(2);
		ataqueFim = true;

	}

	IEnumerator ShotsPD(){
        
        yield return new WaitForSeconds (pausaTiros);
		Instantiate (balaCerb, pontoD.position, pontoD.rotation);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        animator.SetTrigger("OnShoot");
        yield return new WaitForSeconds(pausaTiros);
		Instantiate (balaCerb, pontoD.position, pontoD.rotation);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        animator.SetTrigger("OnShoot");
        yield return new WaitForSeconds(pausaTiros);
		Instantiate (balaCerb, pontoD.position, pontoD.rotation);
        AudioSource.PlayClipAtPoint(shootsound, playerScript.transform.position, shootVolume);
        animator.SetTrigger("OnShoot");

        yield return new WaitForSeconds(2);
		ataqueFim = true;

	}

	IEnumerator OnCharge(){
		yield return new WaitForSeconds (tempoCharge);
		preparoAndamento = false;
		on_charge = false;
		if (podeAtirar) {			
			AtaqueDestroy ();
		} else {
			StartCoroutine ("LaserActive");
		}
	}

	IEnumerator LaserActive(){
		yield return new WaitForSeconds (tempoLaserAtivar);
		GlobalCerberus.laserPrincipal = true;
		laserAtivo = true;
		timerBeam = 0;
		laserBeam.SetActive(true);
	}

	IEnumerator ReiniciarCiclo(){
		yield return new WaitForSeconds (tempoPosAtq + AdicionalJammer);
		sobreEfeitoJammer = false;
		AdicionalJammer = 0;
		pode_atacar = true;
		ataqueFim = false;
		fimAndamento = false;
		selecionou = false;
		podeAtirar = false;
	}

}
