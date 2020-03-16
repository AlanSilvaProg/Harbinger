using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	[Header("Acessos auxiliares de Transform e Rigid")]
	public Rigidbody2D playerRB; // acesso ao RigidBody do personagem
	public Transform GCheck, DirWJ, DirWJextra; // acesso ao verificador de colisão com o chão e paredes
	[Header("Filtro de layer para detectar o que é chao")]
	public LayerMask isGround; // um filtro de layers do que pode ser considerado chão
	[Header("Variaveis de tempo, velocidade, pulo e vida")]
	public float delayToJump; // tempoPerdao de pulo
	public float velo; // velocidade player
	public float veloScan; // velocidade do player quando estiver com o  scan ativo
	public float timePassive; // tempo para entrar em modo passivo
	public int forceJump; // força de pulo
	[Header("Controle de Dash")]
	public bool canDash; // permissão para execução de dash
	[Header("Controle de velocidade do dash")]
	public float veloDash; // contador de tempo de um uso para outro do dash e sua velocidade
	[Header("Pausa entre dash's e o tempo de duração do Dash")]
	public float timePerJump; // pausa entre os dash
	public float dashDuration; // tempo que permanece em dash
	[Header("Controle do Wall Jump")]
	public bool dirC; // Verifica se há colisão com alguma parede
	public bool dirCextra; // verifica se há colisão com alguma parede como um verificador extra
	public float forceXWJ, forceYWJ; // Forças em X e Y para o Wall Jump
	public float divisorVeloWJ; // Velocidade que fica ao estar em uma parede
	[Header("Utilizar power ups sem coletáveis")]
	public bool enableControl; // variavel de controle

	private __GC__ GCScript; // acesso ao game controller
	[Header("Variavel de controle de pulo, não editar")]
	public bool canJump; // Verifica se há colisão com o chão
	private bool jumpControll, canDoubleJump, walking, permiteLastP, WJstate, WJAstate, onDash, wjJump; // variáveis para auxiliar controle de pulo e caminhada
	private float horizontalInput, timer, dashCount, quantDash; // variáveis para auxiliar controle de caminhada e timer de pulo e Contador de Dash
	private Vector3 lastPosition; // ultima posição em que o personagem esteve colidido com o solo

	[Header("Temporario")]
	public float timeToDie; // timer para executar função de morte, delay pra FeedBack
	private float counterDeathTime; // contador de tempo até limite da morte
	public bool dead; // Verifica se está morto ou não

	[Header("animações e particles")]
    public Animator anim;
    private bool isdead;
    private bool onAir;
  
    //
	public BoxCollider2D playerCol;
    private bool tempCollisionCheck;
    //

	[Header("Linkar a barra de energia que fica de baixo do player")]
	public Canvas barEnergySecundario;
	private float timerBarES;


    void Start(){

        onAir = false;
        isdead = false;
        
		wjJump = false;

		GCScript = FindObjectOfType (typeof(__GC__)) as __GC__;
		WJstate = false;
		WJAstate = false;
		quantDash = 0;

        playerCol = GetComponent<BoxCollider2D> ();

		barEnergySecundario.enabled = false;

    }

    void Update() {

		//if (Loading.load) {
			playerCol.enabled = true;
			playerRB.gravityScale = 3;
		//}

		if (barEnergySecundario.enabled && !Global.onStasi && timerBarES < 3) {
			timerBarES += Time.deltaTime;
		} else if (barEnergySecundario.enabled && !Global.onStasi && timerBarES > 3){
			timerBarES = 0;
			barEnergySecundario.enabled = false;
		}

		if (Global.onStasi) {
			timerBarES = 0;
			barEnergySecundario.enabled = true;
		}

		if (dead) {
			transform.SetParent (null);
		}

		if (Input.GetKey (KeyCode.RightControl)) {
			if (Input.GetKeyDown ("l")) {
				if (!enableControl) {
					enableControl = true;
				} else if (enableControl) {
					enableControl = false;
				}
			}
		}

//Mantendo o objeto ao fazer load de uma nova cena
		DontDestroyOnLoad (this.gameObject);

		Global.onDash = onDash;

		Global.dead = dead;

		Global.target = GameObject.Find("Alvo").transform.position;

        //animações e particles
        anim.SetBool("idleon", !walking);
        anim.SetBool("runon", walking);
        anim.SetBool("dashon", onDash);
        anim.SetBool("isDead", isdead);
		if (Global.stasi == false) {
			anim.SetBool ("walljumpOn", (dirC && dirCextra));
		}
        anim.SetBool("onAir", onAir);
        anim.SetBool("FatalOn", Global.life <= 25);

       

        //checagem se o jogador está no ar
        if (!canJump)
        {
            onAir = true;
        }
        else if (canDoubleJump)
        {
            onAir = true;
        }
        else
        {
            onAir = false;
        }
           // 


        if (Global.life <= 0)
        {
            isdead = true;           
        }
        else
        {
            isdead = false;
        }

//Reiniciando game com fim de vida
		if (Global.life <= 0 && dead && counterDeathTime >= timeToDie) {
			playerRB.velocity = Vector2.zero;
			counterDeathTime = 0;
			playerCol.isTrigger = false;
			playerRB.isKinematic = false;
			Global.recharging = false;

			GCScript.SendMessage ("Load");
		}

		if (Global.life <= 0 && dead) {
			counterDeathTime += Time.deltaTime;
		}

		if (Global.life <= 0 && !dead){
			dead = true;
			counterDeathTime = 0;
			playerCol.isTrigger = true;
			playerRB.isKinematic = true;
		}
			

//Atualizando modeState

		if (Input.GetKeyDown ("a") && !Global.pause && !dead && !Global.onStasi) {
			StopCoroutine ("stateATT");
			StartCoroutine ("stateATT");
		}

//Executando Dash
//Controles... Execução no final do update

        if(!canDash && dashCount < timePerJump) {
            onDash = true;
        }
        else{
            onDash = false;
           
            
        }

		if (dashCount > dashDuration && playerRB.gravityScale != 3) {
			playerRB.velocity = Vector2.zero;
			playerRB.gravityScale = 3;
            
        }

		if (!canDash && dashCount > timePerJump) {
			quantDash++;
			canDash = true;
			dashCount = 0;
            
        }

        if (!canDash)
        {
            dashCount += Time.deltaTime;
            
        }


//Atualizando ultimo Olhar

		if (horizontalInput > 0) Global.lastLook = "dir";
		if (horizontalInput < 0)	Global.lastLook = "esq";

//Atualizando Olhar direção do jogador

		if (horizontalInput > 0 && transform.localScale.x < 0 && !Global.pause && !onDash && !Global.isKnock && !Global.recarregando && !Global.onStasi) flip();
		else
			if (horizontalInput < 0 && transform.localScale.x > 0 && !Global.pause && !Global.isKnock && !Global.recharging && !onDash && !Global.onStasi) flip();


//Pulo 

//Verificação de Chão .. podePular recebe true, caso esteja no chão

		dirCextra = Physics2D.OverlapCircle (DirWJextra.position, 0.2f, isGround);

        canJump = Physics2D.OverlapBox (GCheck.position, new Vector2 (0.6f, 0.1f), 0, isGround);
        
        dirC = Physics2D.OverlapCircle (DirWJ.position, 0.2f, isGround);

// Atualizando variaveis de controle de power ups quando tocar o chao

		if (canJump) {
			anim.SetBool ("jumpon", !canJump);
            timer = 0;
			StopCoroutine ("puloPerdao");
			jumpControll = false;
			canDoubleJump = false; //anulando pulo com tempo de Perdao quando esta sobre o solo
			quantDash = 0;
			wjJump = false;
			if (Global.isKnockOut == true){
				StopCoroutine ("knockEnd");
				Global.isKnock = false;
				Global.isKnockOut = false;
			}
		} 

//Verificação para Pular

		if (!canJump && !jumpControll && timer == 0) StartCoroutine ("puloPerdao"); //tornando o puloPerdao ativo

		if (!canJump && timer == 0) StartCoroutine ("timerizar"); //iniciando timer para perdao de pulo

//Negando pulo de perdão se o double jump estiver ativo para uso

		if (canDoubleJump == true && canJump == false) {	StopCoroutine ("puloPerdao");	jumpControll = false;}

//Cortando pulo quando soltado a tecla space

		if (Input.GetKeyUp ("space") && canDoubleJump && playerRB.velocity.y > 0) {
			playerRB.velocity = new Vector2 (playerRB.velocity.x, playerRB.velocity.y / 1.6f);
		}

//fazendo o segundo pulo
		if (Global.doubleJump && !dead || enableControl && !dead) {
			if (!dirC && Global.wallJump || dirC && !Global.wallJump || !dirC && !Global.wallJump) {
				if (Input.GetKeyDown ("space") && canDoubleJump == true && !Global.pause && !onDash && !Global.recharging && !Global.onStasi && !Global.onRamp && !Global.isKnock) {
					canDoubleJump = false;
					wjJump = false;
					playerRB.velocity = new Vector2 (playerRB.velocity.x, 0);
					playerRB.AddForce (new Vector2 (0, forceJump), ForceMode2D.Force);
					anim.SetTrigger ("doubleJumpOn");
                
				}
			}
	}

//Primeiro pulo com perdao de 1 segundo ao sair do solo
		if (!dirC && Global.wallJump || dirC && !Global.wallJump || !dirC && !Global.wallJump) {
			if (Input.GetKeyDown ("space") && canJump == true && !(Input.GetAxisRaw ("Vertical") < 0) && !Global.pause
			   && !dead && !Global.recharging && !onDash && !Global.onStasi && !Global.onRampa || Input.GetKeyDown ("space")
			   && jumpControll == true && !canJump && !canDoubleJump && lastPosition.y > GCheck.position.y && timer < 1 && !Global.pause
			   && !dead && !Global.recarregando && !onDash && !Global.onStasi && !Global.onRamp) {
				playerRB.velocity = new Vector2 (playerRB.velocity.x, 0);
				playerRB.AddForce (new Vector2 (0, forceJump), ForceMode2D.Force);
				canDoubleJump = true;
				wjJump = false;
				anim.SetBool ("jumpon", true);            
			}
		}

//Movimentação, verificando se o personagem está com a tecla A ou D pressionada

		horizontalInput = Input.GetAxisRaw ("Horizontal");

//Verificação Movimentação Horizontal do Personagem

		if (horizontalInput > 0 && !Global.pause && !dead && !Global.recharging && !onDash || horizontalInput < 0 && !Global.pause && !dead && !Global.recharging && !onDash)
			walking = true;
		else {
			if (!Global.pause && canDash && !Global.isKnock && Time.timeScale == 1) {
				playerRB.velocity = new Vector2 (0, playerRB.velocity.y);
			}
			walking = false;
		}

//Executando Dash
		if (Global.dash && !dead || enableControl && !dead) {
			if (Input.GetKeyDown ("s") && !Global.recharging && canDash && !Global.isKnock && !Global.pause && !Global.onScan && Time.timeScale == 1 && quantDash == 0) {
                playerRB.gravityScale = 0;
				canDash = false; // Cancela a possibilidade de diversos dash's ao mesmo tempo
				dashCount = 0; // reinicia o contador de dash
				StopCoroutine ("stateATT");
				StartCoroutine ("stateATT"); // atualizando o estado de modo de jogo do player
				wjJump = false;
//Aplicando Dash com suas condições
				if (horizontalInput > 0 && dashCount < dashDuration || horizontalInput == 0 && Global.lastLook == "dir" && dashCount < dashDuration) {
					if (dirC && transform.localScale.x > 0) {
						flip ();
						playerRB.velocity = Vector2.left * veloDash;
					} else {
						playerRB.velocity = Vector2.right * veloDash;
					}
					
				}
				if (horizontalInput < 0 && dashCount < dashDuration || horizontalInput == 0 && Global.lastLook == "esq" && dashCount < dashDuration) {
					if (dirC && transform.localScale.x < 0) {
						flip ();
						playerRB.velocity = Vector2.right * veloDash;
					} else {
						playerRB.velocity = Vector2.left * veloDash;	
					}
				}
			}
		}
			
//Wall jump Executando

		if (dirC && dirCextra && Global.wallJump && !dead || dirC && dirCextra && enableControl && !dead) { // Verificando se há algum toque em uma parede

			playerRB.velocity = new Vector2 (playerRB.velocity.x, playerRB.velocity.y / divisorVeloWJ);//Deixando mais lenta a queda ao estar em uma parede
			if (Input.GetKeyDown ("space")) {
				wjJump = true;
				WJstate = true;
                anim.SetTrigger("walljumpOut");
                StopCoroutine ("WJAState");
				StartCoroutine ("WJAState"); // Corritina que bloqueia por um curto tempo a movimentação do personagem
				if (Global.lastLook == "dir") {
					playerCol.enabled = false; // desativando collider para não collidir com a parede em y
					playerRB.velocity = new Vector2 (0, 0); // zerando velocidade para executar pulo
					playerRB.AddForce (new Vector2 (forceXWJ* -1, forceYWJ), ForceMode2D.Force); // aplicando força no pulo
				} else if (Global.lastLook == "esq") {
					playerCol.enabled = false; // desativando collider para não collidir com a parede em y
					playerRB.velocity = new Vector2 (0, 0); // zerando velocidade para executar pulo
					playerRB.AddForce (new Vector2 (forceXWJ, forceYWJ), ForceMode2D.Force);// aplicando força no pulo
				}

                
            }
			playerCol.enabled = true; // ativando collider
		}

		if (Global.dead) {
			playerRB.velocity = Vector2.zero;
		}

	}

	void FixedUpdate(){

		if (walking == true && Time.timeScale == 1 && !Global.isKnock && !Global.pause && canDash && !WJAstate) {
			if (WJstate == true)		WJstate = false;
			if (!Global.onScan) {
				playerRB.velocity = new Vector2 ((horizontalInput * velo * Time.deltaTime), playerRB.velocity.y);
			} else {
				playerRB.velocity = new Vector2 ((horizontalInput * veloScan * Time.deltaTime), playerRB.velocity.y);
			}
		}
        

		if (walking == true && Time.timeScale == 1 && !Global.isKnock && !Global.pause && canDash && !WJAstate) {
			if (!Global.onScan) {
			playerRB.velocity = new Vector2 ((horizontalInput * velo * Time.deltaTime), playerRB.velocity.y);
			} else {
				playerRB.velocity = new Vector2 ((horizontalInput * veloScan * Time.deltaTime), playerRB.velocity.y);
			}
		}

		if (walking == false && !Global.isKnock && !Global.pause && canDash && !WJstate && canJump)
				playerRB.velocity = new Vector2(0, playerRB.velocity.y);

	}

	public void atingido(float dmg){ // Sempre que for atingido essa função será chamada
        anim.SetTrigger("knockOn");
		Global.life -= dmg;
		Global.damage = true;
		StopCoroutine ("stateATT");
		StartCoroutine ("stateATT");
	}

	public void flip(){ // Flipando o personagem para quando mudar de direção 
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		Vector3 theScaleBar = barEnergySecundario.transform.localScale;
		theScaleBar.x *= -1;
		barEnergySecundario.transform.localScale = theScaleBar;
	}

 	IEnumerator knockEnd(){ // Corrotina que encerra o Knock back
		yield return new WaitForSeconds (0.8f);
		Global.isKnock = false;
	}

	IEnumerator WJState(){ // corrotina de atualização do wall jump state
		WJstate = true;
		yield return new WaitForSeconds (0.8f);
		WJstate = false;
	}

	IEnumerator WJAState(){ // corrotina de controle de movimentação nos pulos do wall jump
		WJAstate = true;
		yield return new WaitForSeconds (0.2f);
		WJAstate = false;
	}

	IEnumerator puloPerdao(){ // controle de perdao ao efetuar um salto pouco tempo depois de sair de uma plataforma
		jumpControll = true;
		yield return new WaitForSeconds (delayToJump);
		if (GCheck.position.y < lastPosition.y && canDoubleJump == false)
			canDoubleJump = true;
		jumpControll = false;

	}

	IEnumerator timerizar(){ // timer para controle de pulo
		timer = 0.1f;
		yield return new WaitForSeconds (1);
		timer = 1.5f;
	}

	IEnumerator stateATT(){ // Corrotina que atualiza o estado do personagem entre modo passivo e modo batalha
		Global.modeState = true;
		yield return new WaitForSeconds (timePassive);
		Global.modeState = false;
	}



    //Tratamento de Colisão
    void OnCollisionEnter2D (Collision2D col){
        
        switch (col.gameObject.tag) { // tratando colisões por tag

		case "Plataforma":
			if (!Loading.load) {
				transform.SetParent (col.collider.transform);
			}
			break;

		}
		if (col.collider.sharedMaterial != null && col.collider.sharedMaterial.name == "Rampa") {
			Global.onRampa = true;
		}

	}

	void OnCollisionStay2D (Collision2D col){

		switch (col.gameObject.tag) { // tratando colisões por tag

		case "Plataforma":
			if (!Loading.load) {
				transform.SetParent (col.collider.transform);
			}
			break;

		}

		if (col.collider.sharedMaterial.name == "ChaoInterno") {
			Global.life -= Global.life;
		}


	}

	void OnCollisionExit2D (Collision2D col){
        

        switch (col.gameObject.tag) {
            
		case "Chao":
                lastPosition = GCheck.position;
                break;

		case "Plataforma":
			transform.SetParent (null);
			lastPosition = GCheck.position;
			break;

		}

		if (col.collider.sharedMaterial != null && col.collider.sharedMaterial.name == "Rampa") {
			Global.onRampa = false;
		}

	}

//Tratamento de Trigger's
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Respawn") {
			Global.life = 0;
		}
	}
		
}