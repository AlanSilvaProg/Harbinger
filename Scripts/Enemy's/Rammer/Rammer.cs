using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rammer : MonoBehaviour
{

	[Header ("Vida do Rammer")]
	[Range (0, 100)]
	public float vida;
	// vida do rammer
	[Header ("Força de Knock e dano")]
	public float dano;
	// dano que o rammer causa ao jogador
	public float forceX;
	// força em x do knock
	public float forceY;
	// força em y do knock
	[Header ("Pause Entre Danos")]
	public float pausaDano;
	// pausa entre cada dano
	[Header ("Valor de divisão na velocidade do ataque")]
	public float veloATKDivisor;
	// divisão de velocidade do ataque
	[Header ("Velocidade Padrão do Rammer")]
	public float veloRammerMov;
	// velocidade do rammer padrão
	[Header ("Tempo descanso após realizar ataque")]
	public float descanso;
	// tempo de descanso após ataque
	[Header ("Tempo de preparado para realizar ataque")]
	public float tempoPreparo;
	// tempo se preparando para realizar o ataque, mirando no jogador
	[Header ("Valor adicional para o descanso com jammer")]
	public float JammerAdicional;
	// tempo extra causado pelo jammer
	[Header ("Adicionar 2 pontos padrões de movimento fora de batalha")]
	public GameObject[] pontoPadraoMov;
	// pontos de movimento padrão, A e B

	public bool podeAlcancar;
    // controle de alcanse do jogador
    public Animator animator;
	private Player playerScript;
	private Rigidbody2D RammerRB;
	private Vector3 RammerRef, visaoInicial, posicaoRam;
	private Vector3 alvo;
	private float anguloAlvo, timerDesc;
	private Vector2 posicaoRamm;
	private bool prepararAtaque = false, atacando = false, descansando = false, controll = true;
	private bool pauseDmg;
	private float timerJammerEfct = 0;
	private bool a, b;
	public float chargeSpeed;
	private float tempAngle;
	private Vector3 startcharge;
	public float maxDist;
	public LayerMask paraEm;
    public ParticleSystem b00mfx;
    public AudioClip b00msound;
    public float b00mVolume = .3f;
    // public AudioClip hitsound;


    // Use this for initialization
    void Start ()
	{
        
		posicaoRamm = new Vector2 (transform.position.x, transform.position.y);
		RammerRB = GetComponent<Rigidbody2D> ();
		visaoInicial = transform.right;
		alvo = pontoPadraoMov [0].transform.position;

	}
	
	// Update is called once per frame
	void Update ()
	{
        
        animator.SetBool("Locked", !descansando);
        if ( prepararAtaque == true)
        {
            animator.SetTrigger("Locking");
        }

        if (descansando == true)
        {
            animator.SetTrigger("Unlocking");
        }



        if (vida <= 0)
        {
            animator.SetTrigger("Morreu");
            morte();
        }


//Guardando Transform do Rammer (Unity Pede isso para poder modificar transform)
		RammerRef = transform.position;

//Acessando script Player
		playerScript = Global.PlayerAcess;

//Verificando Angulos
		RammerRef.z = playerScript.transform.position.z;

		transform.position = RammerRef;

//movimentando em um padrão caso player não esteje no alcance
		if (a == false && b == false)
			a = true;

		if (!Global.pause) {
		
			if (!podeAlcancar && !descansando && !atacando && !prepararAtaque) {
				OlharAlvo ();
				MoverPadrao ();
			}
		    

			if (descansando && !prepararAtaque && !atacando) {
				Descansar (descanso);

			}

//Condição para guardar uma posição para quando o Rammer for retornar após ataque	
			if (prepararAtaque && !atacando) {
				OlharAlvo ();
				posicaoRamm = new Vector2 (transform.position.x, transform.position.y);
				posicaoRam = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			}

//Atualizando alvo enquanto não estiver no ataque(para ter um ataque em linha reta sem modificação de destino no caminho)
			if (atacando && !descansando) {
				Ataque ();
			}

			if (podeAlcancar && !prepararAtaque && !atacando && !descansando) {
				PreparoDeAtaque (tempoPreparo);
			}
		

//Olhando para o alvo enquanto prepara para atacar
			if (prepararAtaque && !atacando && !descansando) {
				alvo = GameObject.Find ("Alvo").transform.position;
				OlharAlvo ();
			}	
		}
	}

	//Função que realiza o inicio do preparo de ataque
	void PreparoDeAtaque (float tmpPreparo)
	{
        
		prepararAtaque = true;


		if (controll && prepararAtaque)
			StartCoroutine ("preparoFinal", tmpPreparo); // chamando corrotina que encerra preparo e inicia o ataque
	}

	void OlharAlvo ()
	{
		//transform.right = alvo - transform.position;

//		print (alvo);
		Vector3 difference = alvo - transform.position;
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationZ);


	}

	void Ataque ()
	{
			
		transform.Translate ((chargeSpeed) * Time.deltaTime,0, 0);
		if (Vector3.Distance (transform.position, startcharge) > maxDist) {
			descansando = true;
			atacando = false;
            animator.SetBool("Voando", true);
        }
        else
        {
            animator.SetBool("Voando", false);
        }

	}

	void Descansar (float tempo)
	{
		
		if (timerJammerEfct > 0) {
			tempo += timerJammerEfct;
			timerJammerEfct = 0;
		}

		if (timerDesc < tempo) {
			timerDesc += Time.deltaTime;
		} else {
			if (transform.position.x == posicaoRamm.x && transform.position.y == posicaoRamm.y) {
				descansando = false;
				timerDesc = 0;
				//transform.right = visaoInicial;
			} else {
				alvo = posicaoRamm;
				//transform.right = posicaoRam - (Vector3)transform.position;
				transform.position = Vector2.MoveTowards (transform.position, posicaoRamm, veloRammerMov * 4f * Time.deltaTime);
			}
		}

	}

	void MoverPadrao ()
	{

		if (a && Mathf.Abs (pontoPadraoMov [0].transform.position.x - transform.position.x) >= 0.3f) {
			alvo = pontoPadraoMov [0].transform.position;

			transform.position = Vector2.MoveTowards (transform.position, new Vector2 (pontoPadraoMov [0].transform.position.x, pontoPadraoMov [0].transform.position.y), veloRammerMov * Time.deltaTime);
		} else {
			a = false;
			b = true;
		}
		if (b && Mathf.Abs (pontoPadraoMov [1].transform.position.x - transform.position.x) >= 0.2f) {
			alvo = pontoPadraoMov [1].transform.position;
			transform.position = Vector2.MoveTowards (transform.position, new Vector2 (pontoPadraoMov [1].transform.position.x, pontoPadraoMov [1].transform.position.y), veloRammerMov * Time.deltaTime);
		} else {
			a = true;
			b = false;
		}

	}

	public void damage ()
	{
		if (!pauseDmg) {
			StartCoroutine ("pauseToDmg");
            //Global.life -= dano;
            Global.PlayerAcess.SendMessage("atingido", dano);
            playerScript.StartCoroutine ("knockEnd");
			Global.isKnock = true;
			if (Global.playerRB.position.x > RammerRB.position.x) {
				Global.playerRB.velocity = new Vector2 (0, 0);
				Global.playerRB.AddForce (new Vector2 (forceX, forceY), ForceMode2D.Force);
			} else if (Global.playerRB.position.x < RammerRB.position.x) {
				Global.playerRB.velocity = new Vector2 (0, 0);
				Global.playerRB.AddForce (new Vector2 (forceX * -1, forceY), ForceMode2D.Force);
			}
		}
	}

	public void atingidoBala (float dmgBala)
	{
        animator.SetTrigger("Hit");
        vida -= dmgBala;
        //AudioSource.PlayClipAtPoint(hitsound, Global.playerRB.transform.position, b00mVolume);
    }

    public void atingidoJammer (float dmgBala)
	{
		if (timerJammerEfct == 0)
			timerJammerEfct += JammerAdicional;
		vida -= dmgBala;
	}

	IEnumerator pauseToDmg ()
	{
		pauseDmg = true;
		yield return new WaitForSeconds (pausaDano);
		GetComponent<CircleCollider2D> ().enabled = true;
		pauseDmg = false;
		Global.isKnock = false;
	}

	IEnumerator preparoFinal (float tempo)
	{
		controll = false;
		yield return new WaitForSeconds (tempo);
		prepararAtaque = false;
		controll = true;
		atacando = true;
		float ydif = Mathf.Abs (transform.position.y - alvo.y);
		if (ydif < 1 && ydif > 0) {
			tempAngle = 0;
		} else {
			tempAngle = (Mathf.Abs (transform.position.x - alvo.x) / Mathf.Abs (transform.position.y - alvo.y));
		}
		startcharge = transform.position;
		//print (tempAngle);
	

	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Player") {
			if (atacando) {
				GetComponent<CircleCollider2D> ().enabled = false;
				damage ();
				descansando = true;
				atacando = false;
			}
		}


		if (paraEm == (paraEm | (1 << col.gameObject.layer))) {
			descansando = true;
			atacando = false;

		}
	}

	void morte(){
		if(Apolo.APOLOON){
			Apolo.quantidadeInimigosOn -= 1;
		}
        //tocar som de torret sendo destruida e tocar animações necessárias
        Instantiate(b00mfx, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(b00msound, playerScript.transform.position, b00mVolume);
        Destroy(gameObject.transform.parent.gameObject);
	}



}
