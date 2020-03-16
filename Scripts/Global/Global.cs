using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public Player playerScript;
	public __GC__ GCScript;
	public PlayerGun playerGunScript;
	public PlayerTime playerTimeScript;
	public GameObject camera;

	public static GameObject cameraa;

	/// <summary>
	/// Variáveis de auxilo para o Rômulo aplicar sons
	/// </summary>

	public static int theme; // 1 - Tier 1 .  2 - Tier 2 . 3 - Power Ups do Tier 1 . 4 - Power Up do tier 2 . 5 - BossBattle Cerberus . 6 - BossBattle APOLO
	public static int randomValue; // Randomico de 1 a 5 sempre que avançar um nivel
	public static int qual_metade_Cerb; // separado em 4 quartos de 100, a vida tem 4 fases , 100 - 75- 50 - 25
	public static int qual_metade_APOLO; // separado em 4 quartos de 100, a vida tem 4 fases , 100 - 75- 50 - 25
	public static int Especial_Boss_Cerb; // variavel com valor negativo e positivo dependendo de quando é executado o ataque especial
	public static int Especial_Boss_APOLO; // variavel com valor positivo dependendo de quando é executado o ataque especial
	public static int OutroAtq_Boss_Cerb; // variavel com valor positivo dependendo de quando é executado o ataque comum
	public static int OutroAtq_Boss_APOLO; // variavel com valor positivo dependendo de quando é executado o ataque comum

	/// <summary>
	/// Variáveis de auxilo para o Rômulo aplicar sons
	/// </summary>


	[Header("Esconder itens de teste das cenas ao dar play")]
	public bool stealth;

	public static string[] title, history; 

	public static bool stealthTestes;
	public static bool firstSum = true;

	public static string[] tuto = new string[4];

	public static bool pause, gameOver;
	public static Rigidbody2D playerRB;
	public static float life;
	public static float energy; 
	public static bool isKnock;
	public static bool isKnockOut;
	public static bool modeState;
	public static bool recharging;
	public static bool onScan;
	public static bool onStasi;
	public static bool onDash;
	public static bool dash = false;
	public static bool wallJump = false;
	public static bool doubleJump = false;
	public static bool groundPound = false;
	public static bool stasi = true;
	public static bool jammer = false;
	public static bool greande = false;
	public static string lastLook;
	public static int currentTuto;
	public static int exitInfo;
	public static bool onRamp;
	public static bool dead;
	public static float lifeMax = 100;
	public static float energyMax = 100;

	public static bool damage;
	public static Vector3 lastPositionToLoad;

	public static int[] bulletsOnScene , ammo;
	public static int equippedGun;
	public static bool grenadeOnScene;

	public static __GC__ GCAcess;
	public static Player PlayerAcess;

	public static Vector3 target;
	public static float damageFrag;
	public static int quantFrag;
	public static float forceX, forceY; 


	public static bool loadDeath;

	void Start(){
		
		GCAcess = GCScript;
		PlayerAcess = playerScript;

		if (!stealthTestes) {
			stealthTestes = stealth;
		} else {
			stealth = stealthTestes;
		}
			
		pause = gameOver = isKnock = isKnockOut = modeState = damage = false;
		playerRB = playerScript.playerRB;
		life = 100;
		energy = 100;
		lastLook = "dir";

		bulletsOnScene = new int[3]; 
		ammo = new int[3];
		playerGunScript.initialAmmo (0);
		playerGunScript.initialAmmo (2);
		playerGunScript.initialAmmo (1);
        equippedGun = playerGunScript.equippedGun;

		cameraa = camera;
	
	}
}
