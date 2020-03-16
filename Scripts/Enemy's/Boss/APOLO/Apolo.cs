using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Apolo : MonoBehaviour {

	public GameObject backbar;
	public GameObject lifebar;
	private Image lifebarimg;
	private float vidaInicial;

	[Header("Vida do APOLO")]
	public float vida; // vida do Apolo
	[Header("Dano de contato com o jogador")]
	public float danoContato; // Dano que o APOLO causa ao collidir com o Player
	[Header("Dano que APOLO Recebe por disparo do player")]
	public float danoPlayer; // dano que o APOLO Recebe por cada disparo que o atinge durante seu modo Spawner
	[Header("Tempo para o APOLO Despertar")]
	public float tempoParaIniciar; // tempo que levará até o APOLO Acordar e iniciar seu ciclo de ataques
	[Header("Valores de Knock aplicado no player")]
	public float forceX;
	public float forceY;

	[Header("Controle de transição entre Ataques")]
	public float ataque_Transicao;

	public bool Modo_Andar = true;
	public bool andando; // fica true quando se está em movimento para um novo destino
	[Header("Controle do Modo Andar do APOLO")]
	[Header("pontos para movimentação do APOLO")]
	public Transform pontoDireito; // ponto padrão direito para movimentação 
	public Transform pontoEsquerdo; // ponto padrão esquerdo para movimentação
	[Header("Ponto em que o APOLO fica durante spawn de inimigos")]
	public Transform pontoBaixo; // usado pra o Spawn de Inimigos, Vulnerabilidade
	[Header("Velocidade de movimento do APOLO")]
	public float veloAndar;

	public bool Modo_Projeteis;
	public bool preparando_Pro; // preparando para realizar instancias dos orbs
	public bool andamento_Pro; // Fica true sempre que está com os Orbs ativos na cena
	private Transform destino;
	[Header("Controle do Modo Projeteis do APOLO")]
	[Header("Tempo que ficará ativo os Orbs Emissores")]
	public float tempoOrb;
	[Header("PreFabs dos Orbs Emissores")]
	public GameObject emissorDireito; // Orb Emissor de projeteis que será instanciado por x segundos no lado direito
	public GameObject emissorEsquerdo; // Orb Emissor de projeteis que será instanciado por x segundos no lado esquerdo
	[Header("Transform de Spawn dos Orbs")]
	public Transform DireitoSpawn; // pontos de referencia para spawnar Orb direito na posição correta
	public Transform EsquerdoSpawn; // pontos de referencia para spawnar Orb esquerdo na posição correta

	[Header("Tempo adicional de vida do Orb para cada dificuldade avançada")]
	public float timeOrbAdc; // tempo que será aumentado na vida do Orb quando a porcentagem de cada nivel do APOLO for atingida

	public bool Modo_Spawner; 
	public bool preparando_Spawn; // preparando para realizar os conjuros
	public bool conjurando; // Controle de conjuro, diz se está ou não conjurando
	[Header("Controle do Modo Spawner do APOLO")]
	[Header("Tempo vulneravel durante o processo de ataque")]
	public float vulnerabilidade;
	[Header("Quantidade de inimigos que poderão ser spawnados")]
	public int quantSpawnar; // quantidade de inimigos iniciais que serão instanciados nesse ataque
	[Header("Será aumentado quantidade de inimigos")]
	[Header("por cada vez que a porcentagem for tirada de APOLO")]
	public int porcentoVida; // porcentagem de vida que o APOLO deve perder para que haja aumento na dificuldade / quantidade de inimigos
	[Header("Quantidade adicional por porcento tirado")]
	public int adicionalInimigo; // quantidade de inimigos que serão adicionados ao total que será spawnado na cena durante o ataque
	[Header("Máximo de inimigos em cena para permitir novas instancias")]
	public int maxInimigosOn; // Máximo de inimigos que podem estar vivo para que o ataque ocorra
	[Header("Máximo adicional por porcento tirado")]
	public int maxAdicionalIni;

	[Header("Spawn e PreFabs de inimigos para serem conjurados")]
	public GameObject RoombaEnemy; // Prefab do Roomba
	public Transform[] RoombaSpawn = new Transform[3]; // possiveis spawns do roomba
	public GameObject Bomba; // Prefab da bomba
	public Transform[] BombaSpawn = new Transform[3]; // possiveis spawns da bomba
	public GameObject Rammer; // Prefab do Rammer
	public Transform[] RammerSpawn = new Transform[3]; // possiveis spawns do Rammer
	public GameObject PatternEmiters; // Prefab do Pattern Emiter
	public Transform[] PatternSpawn = new Transform[3]; // possiveis spawns do Pattern

	[Space(10)]
	public Animator APOLOANIM;
	public Animator APOLOANIMCOMP;

	public Transform apoloo;

//Variaveis secundarias de Controle do inicio do APOLO
	private bool on; // Variavel que controla a permissão dos movimentos do APOLO
	private bool iniciando; // variavel que controla o tempo em que se passa o processo de inicio do apolo
	private float timerInicio; // contador de tempo durante a sua iniciação

	private float vidaInicio;
	private int Level = 0;

	private bool nivelAvançoHP1; // ativa quando o primeiro nivel de dificuldade é alcançado
	private bool nivelAvançoHP2; // ativa quando o segundo nivel de dificuldade é alcançado
	private bool morte; // variavel de morte

	private float timerSpawn; // tempo spawnando
	private bool fimSpawner; // quando acaba o spawner

	private float timerTransicao; // contador para transicao de ataques

	public static float OrbTime; // tempo de vida do orb
	public static bool APOLOON = false; // Controle para inimigos apenas quando APOLOON é true
	public static bool emissorD = false; // Controle de Emitters spawnados pelo APOLO do lado Direito
	public static bool emissorE = false; // Controle de Emitters spawnados pelo APOLO do lado Esquerdo
	public static float quantidadeInimigosOn = 0; // Controle Global de quantos inimigos que o Apolo spawnou ainda estão na cena

	private bool BackOn, ATKOn, ConjureOn, FrontOn; // configurações do animator

	private Player playerScript; // acesso do playerScript para aplicação de Knock ao entrar em contato

    private float timerFim = 0;

    // Use this for initialization
    void Start () {

		iniciando = true; // Deixando possibilitado a inicialização com um delay DO APOLO (Para aplicações de FeedBack Visual e Sonoro)
		timerSpawn = 0;

		OrbTime = tempoOrb;
		andando = false;

		vidaInicio = vida;
		lifebarimg = lifebar.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (gameObject.activeSelf) {
			if (!backbar.activeSelf) {
				backbar.SetActive (true);
			}

			lifebarimg.fillAmount = vida / vidaInicio;
		} else {
			backbar.SetActive (false);
		}

		if (BackOn && apoloo.localScale.x < 0) {
			BackOn = false;
			FrontOn = true;
		} else if (FrontOn && apoloo.localScale.x < 0) {
			FrontOn = false;
			BackOn = true;
		}
		APOLOANIMCOMP.SetBool("BackOn", BackOn);
		APOLOANIMCOMP.SetBool("ATKOn", ATKOn);
		APOLOANIMCOMP.SetBool("ConjureOn", ConjureOn);
		APOLOANIMCOMP.SetBool("FrontOn", FrontOn);

		APOLOON = on;

        //Armazenando em qual Quarto de vida o APOLO está

        if (vida <= 0)
        {
            if (timerFim < 3)
            {
                timerFim += Time.deltaTime;
            }
            else
            {
                Destroy(Global.GCAcess.gameObject);
                Destroy(Global.PlayerAcess.gameObject);
                SceneManager.LoadScene("Créditos");
            }
        }

        if (vida < 0){
			Global.qual_metade_APOLO = 0;
            APOLOANIMCOMP.SetTrigger("OnDeath");
        }

		if (((vidaInicio * vida) / 100) > 0 && ((vidaInicio * vida) / 100) < ((vidaInicio * 25) / 100)) {
			Global.qual_metade_APOLO = 1;
		}

		if (((vidaInicio * vida) / 100) > ((vidaInicio * 25) / 100) && ((vidaInicio * vida) / 100) < ((vidaInicio * 50) / 100)) {
			Global.qual_metade_APOLO = 2;
		}

		if (((vidaInicio * vida) / 100) > ((vidaInicio * 50) / 100) && ((vidaInicio * vida) / 100) < ((vidaInicio * 75) / 100)) {
			Global.qual_metade_APOLO = 3;
		}

		if (((vidaInicio * vida) / 100) > ((vidaInicio * 75) / 100)) {
			Global.qual_metade_APOLO = 4;
		}

		/////////////////////////////////////////////

		if (Level == 6 && ((vidaInicio * vida) / 100) <= 0 ) {
			APOLOANIM.SetTrigger ("NextLvl");
		}

		if (Level == 5 && ((vidaInicio * vida) / 100) < ((vidaInicio * 15) / 100)) {
			APOLOANIM.SetTrigger ("NextLvl");
			Level = 6;
		}

		if (Level == 4 && ((vidaInicio * vida) / 100) < ((vidaInicio * 30) / 100)) {
			APOLOANIM.SetTrigger ("NextLvl");
			Level = 5;
		}

		if (Level == 3 && ((vidaInicio * vida) / 100) < ((vidaInicio * 40) / 100)) {
			APOLOANIM.SetTrigger ("NextLvl");
			Level = 4;
		}

		if (Level == 2 && ((vidaInicio * vida) / 100) < ((vidaInicio * 60) / 100)) {
			APOLOANIM.SetTrigger ("NextLvl");
			Level = 3;
		}

		if (Level == 1 && ((vidaInicio * vida) / 100) < ((vidaInicio * 70) / 100)) {
			APOLOANIM.SetTrigger ("NextLvl");
			Level = 2;
		}

		if (Level == 0 && ((vidaInicio * vida) / 100) < ((vidaInicio * 80) / 100)) {
			APOLOANIM.SetTrigger ("NextLvl");
			Level = 1;
		}

		/////////////////////////////////////////////

//Acessando Script do player através do acesso rápido do Global

		if (playerScript == null) {
			playerScript = Global.PlayerAcess;
		}

//Negando valor de variavel musical fora do ataque
		if (!emissorD && !emissorE && Global.OutroAtq_Boss_APOLO > 0) {
			Global.OutroAtq_Boss_APOLO = 0;
		}

              //Comportamento//
		/////////////////////////////
/// Condição para rodar Ciclo do APOLO
		if (on && vida > 0 && !Global.pause) {
			
//Verifica em que nivel se encontra o APOLO de acordo com seu porcentual de vida
			//Verificando nivel 2;
			if (vida < ((((vidaInicial*2) * porcentoVida) / 100) - vidaInicial) && !nivelAvançoHP1) {
				quantSpawnar += adicionalInimigo;
				maxInimigosOn += maxAdicionalIni;
				nivelAvançoHP2 = true;
			}

			//Verificando nivel 1;
			if (vida < (((vidaInicial * porcentoVida) / 100) - vidaInicial) && !nivelAvançoHP1) {
				quantSpawnar += adicionalInimigo;
				maxInimigosOn += maxAdicionalIni;
				nivelAvançoHP1 = true;
			}

//Condição para reiniciar o ciclo do APOLO
			if (Modo_Spawner && preparando_Spawn && conjurando) {
				if (timerSpawn < vulnerabilidade) { // Verificando se APOLO ainda está vulneravel
					timerSpawn += Time.deltaTime; // somando no timer o tempo que ficou vulneravel
				} else {
					ConjureOn = false;
					Global.OutroAtq_Boss_APOLO = 0;
					Global.Especial_Boss_APOLO = 0;
					if (timerTransicao > ataque_Transicao) {
						Modo_Spawner = false; // Encerrando o ciclo de conjuramento de Inimigos
						conjurando = false; // Encerrando o ciclo de conjuramento de Inimigos
						Modo_Andar = true; // iniciando ciclo de caminhada
						timerSpawn = 0; // resetando timer para o próximo ciclo
						timerTransicao = 0;
					}else {
						timerTransicao += Time.deltaTime;
					}
				}
			}

//Condição para Spawnar os inimigos na cena com o poder do APOLO
			if (Modo_Spawner && preparando_Spawn && conjurando) {
				ConjureOn = true;
				Global.OutroAtq_Boss_APOLO = 0;
				Global.Especial_Boss_APOLO = 1;
				SpawnarInimigos (); // Função para spawnar inimigos
			}

			if (Modo_Spawner && preparando_Spawn && !conjurando) {
				MoverSpawn (); // função para mover APOLO até determinada posição
			}

//Condição para iniciar o processo de spawn de inimigos com o poder do APOLO
			if (Modo_Spawner && !preparando_Spawn) {
				preparando_Spawn = true;
				destino = pontoBaixo;
				destino.position = new Vector2 (transform.position.x, pontoBaixo.position.y);
			}

//Condição para reiniciar o ciclo do APOLO
			if (Modo_Projeteis && andamento_Pro) {
				if (!emissorD && !emissorE) {
					ATKOn = false;
					if (timerTransicao > ataque_Transicao) {
						Modo_Projeteis = false;
						andamento_Pro = false;
						Modo_Andar = true;
						timerTransicao = 0;
					}else {
						timerTransicao += Time.deltaTime;
					}
				}
			}

			if (Modo_Projeteis && !andamento_Pro) {
				Global.OutroAtq_Boss_APOLO = 1;
				Global.Especial_Boss_APOLO = 0;
				andamento_Pro = true;
				ATKOn = true;
				SpawnarOrbs ();
			}

//Condição para iniciar um novo Ataque
			if (!Modo_Andar && andando) {
				if (quantidadeInimigosOn < maxInimigosOn) { // Condição para realizar ataque de spawn de inimigos com o poder de APOLO
					if (timerTransicao > ataque_Transicao) {
						Modo_Spawner = true;
						andando = false;
						timerTransicao = 0;
					}else {
						timerTransicao += Time.deltaTime;
					}
				} else { // Se o ataque de spawn não é possivel, executa o ataque de projeteis através dos orbs em suas mãos
					if(timerTransicao > ataque_Transicao) {
						Modo_Projeteis = true;
						andando = false;
						timerTransicao = 0;
				}else {
					timerTransicao += Time.deltaTime;
				}
				}
			}

//Condição para se mover entre um ponto e outro durante a pausa entre ataques
			if (Modo_Andar && andando) {
				MoverDestino ();
			}

//Condição para iniciar sua movimentação no cenário
			if (Modo_Andar && !andando) {
				SelecionarDestino ();
			}

		/////////////////////////////
		}
//Iniciando o APOLO caso esteja off
		if (iniciando && timerInicio > tempoParaIniciar) { // verificando se já pode ativar
			iniciando = false; 
			on = true; // ativando
			Modo_Andar = true;
		} else {
			timerInicio += Time.deltaTime; // somando timer até que possa ativar o APOLO
		}

		//Quando sua vida zera, APOLO morre... Sem Função de morte ainda, para decisão de como proceder após ele morrer(CutScene, Créditos ou ???)
		if (vida <= 0) {
			morte = true;
		}
		
	}

//Função que instancia um inimigo em determinados pontos do cenário
	void SpawnarInimigos (){
//deixando true o conjurando para que haja repetição da função até que chegue no valor limite de inimigos
		conjurando = true;

//Randomizando o inimigo que será spawnado 0= Roomba, 1 = Bomba, 2 = Rammer, 3 = PatternEmiters
		int randomEnemy = (int)Random.Range (0, 4);

		if (quantidadeInimigosOn < maxInimigosOn) {
			switch (randomEnemy) {
			case 0:
				Instantiate (RoombaEnemy, RoombaSpawn [(int)Random.Range (0, 3)].position, Quaternion.identity);
				quantidadeInimigosOn += 1;
				break;
			case 1:
				Instantiate (Bomba, BombaSpawn[(int)Random.Range (0, 3)].position, Quaternion.identity);
				quantidadeInimigosOn += 1;
				break;
			case 2:
				Instantiate (Rammer, RammerSpawn[(int)Random.Range (0, 3)].position, Quaternion.identity);
				quantidadeInimigosOn += 1;
				break;
			case 3:
				Instantiate (PatternEmiters, PatternSpawn[(int)Random.Range (0, 3)].position, Quaternion.identity);
				quantidadeInimigosOn += 1;
				break;
			}
		} else {
			preparando_Spawn = false; // Finalizando o ciclo de Spawns após atingir limite
		}


	}

//Função para mover o APOLO até a posição de Vulnerabilidade
	void MoverSpawn	(){
		
//Verificando distancia entre o APOLO e o seu destino e caminhando até ele
		if (Vector2.Distance (transform.position, destino.position) > 1) {
			transform.position = Vector2.MoveTowards (transform.position, destino.position, veloAndar * Time.deltaTime);
		} else {
			conjurando = true; // finalizando ciclo de caminhar pelo cenário e iniciando conjuramento de inimigos
		}

	}

	void SpawnarOrbs (){

		int randomOrb = (int)Random.Range (0, 2);

//Verificando se já não existe algum emissor spawnado para poder spawnar um novo
		if (randomOrb == 0) {
			if (!emissorD) {
				Instantiate (emissorDireito, DireitoSpawn);
				emissorD = true; // Evitando problemas de spawn duplo
			}
		}

//Verificando se já não existe algum emissor spawnado para poder spawnar um novo
		if (randomOrb == 1) {
			if (!emissorE) {
				Instantiate (emissorEsquerdo, EsquerdoSpawn);
				emissorE = true; // Evitando problemas de spawn duplo
			}
		}
	}

//Função para mover APOLO até um novo ponto pré selecionado
	void MoverDestino (){
//Verificando a distancia minima para considerar dentro do destino e movendo até ele
		if (Vector2.Distance (transform.position, destino.position) > 1) {
			transform.position = Vector2.MoveTowards (transform.position, destino.position, veloAndar * Time.deltaTime);
		} else {
			Modo_Andar = false; // finalizando ciclo de caminhar pelo cenário
			FrontOn = false;
			BackOn = false;
		}
	}

//Selecionando o destino que o APOLO deve ir
	void SelecionarDestino (){
		
		if (Vector2.Distance (transform.position, pontoDireito.position) < 1) {
			destino = pontoEsquerdo;
			BackOn = true;
		} else if (Vector2.Distance (transform.position, pontoEsquerdo.position) < 1) {
			destino = pontoDireito;
			FrontOn = true;
		} else {
			destino = pontoDireito;
			FrontOn = true;
		}

		andando = true;

	}

//Função executada através de interação entre Scripts quando uma bala comum do player atinge APOLO
	public void atingidoBala (float dmgBala)
	{
		if (Modo_Spawner) {
			////////
			/// Espaço para FeedBack de bala comum
			///////
			vida -= danoPlayer;
			APOLOANIM.SetTrigger ("OnHit");
		}
	}

//Função executada através de interação entre Scripts quando uma bala Jammer do player atinge APOLO
	public void atingidoJammer (float dmgBala)
	{
		if (Modo_Spawner) {
			////////
			/// Espaço para FeedBack de jammer
			///////
			vida -= danoPlayer / 2;
			APOLOANIM.SetTrigger ("OnHit");

		}
	}

//Verificando se há contato entre APOLO e o Player para poder aplicar dano e knock
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Global.life -= danoContato;
			playerScript.StartCoroutine ("knockEnd");
			Global.isKnock = true;
			if (Global.playerRB.position.x > transform.position.x) {
				Global.playerRB.velocity = new Vector2 (0, 0);
				Global.playerRB.AddForce (new Vector2 (forceX, forceY), ForceMode2D.Force);
			} else if (Global.playerRB.position.x < transform.position.x) {
				Global.playerRB.velocity = new Vector2 (0, 0);
				Global.playerRB.AddForce (new Vector2 (forceX * -1, forceY), ForceMode2D.Force);
			}
		}
		}

}