using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moviment : MonoBehaviour
{


	public Transform[] destinys = new Transform[2];
	public Transform platTransf;
	public float velo;
	public int timerTrocaDestino;
	public int destiny;
	private bool move, trade;

	public int direction = 1;

	public bool isSpawner = false;
	public bool isPlayerDetecting = false;
	public bool isCiclico = false;
    public bool startInDest = true;
	public bool isJammerLocked = false;
	public bool DoesItReturn = true;

	private SpriteRenderer spr;

	// Use this for initialization
	void Start ()
	{
		spr = GetComponent<SpriteRenderer>();

		if (isCiclico && isSpawner) {
			print ("Favor lembrar que isCiclico e IsSpawner são incompativeis, vc fez esse codigo, para de ser burro!");
		}

		move = false; // se for verdadeiro ele avança para o destino
		
        if (startInDest == true)
        {
            platTransf.position = destinys[destiny].position; // posição inicial da plataforma
            
        }

		if (isPlayerDetecting) {
			
		}
		
		
		///StartCoroutine ("mudarPlatDestino");//iniciando o ciclo da movimentação da plataforma

	}
	
	// Update is called once per frame
	void Update ()
	{
		
//		Debug.Log (platTransf);
//Verificando se a plataforma não está no destino para que haja prosseguimento no ciclo do move
		if (!DoesItReturn && destiny == destinys.Length - 1) {

		} else {
			if (platTransf.position != destinys [destiny].position) {
				move = true;
			} else {
				move = false;
				trade = true;
				if (!isJammerLocked) {
					if (!IsInvoking ("ChangeDestinyPlatform")) {
						Invoke ("ChangeDestinyPlatform", timerTrocaDestino);
					}
				} else if (destiny != 0) {
					if (!IsInvoking ("ChangeDestinyPlatform")) {
						Invoke ("ChangeDestinyPlatform", timerTrocaDestino);
					}
				} else {
					spr.color = new Color32 (0, 255, 55, 255);
				}
		
			}
		}

//fazendo com que avançe para o novo destino

		if (isSpawner && direction < 0) {
			destiny = 0;
			platTransf.SetPositionAndRotation (destinys [destiny].position, destinys [destiny].rotation);
			direction = 1;
		} else {
			platTransf.position = Vector3.MoveTowards (platTransf.position, destinys [destiny].position, velo * Time.deltaTime);
		}

        if (isPlayerDetecting)
        {
            if(Global.morto == true && direction == 1)
            {
                direction = -1;
            }
        }
	}

	void ChangeDestinyPlatform()
	{
		if (destiny == destinys.Length - 1 || destiny == 0) {
			if (isPlayerDetecting) {
				if (destiny == 0) {
					foreach (Transform child in transform) {
						if (child.tag.Equals ("Player")) {
							destiny = 1;
							direction = 1;
						}

					}

				} else if (destiny == destinys.Length - 1 ) {

					direction = -1;
					
					destiny = destinys.Length - 2;
					foreach (Transform child in transform) {
						if (child.tag.Equals ("Player")) {
							
							destiny = destinys.Length - 1;
							break;
						}

					}

				} else {
					resolveTurning ();
				}
		
			} else {
				resolveTurning ();
			}
		}/* else if (isPlayerDetecting) {
			if (Vector3.Distance(platTransf.position,Global.PlayerAcess.transform.position) > 5) {
				direcao = -1;
			} 
				destino += 1 * direcao;
			}*/
		else {
			destiny += 1 * direction;
		}

		trade = false;

	}

	public void resolveTurning ()
	{
		if (!isCiclico) {
			direction = (destiny == 0) ? 1 : -1;
			destiny += 1 * direction;

		} else {
			destiny = (destiny == 0) ? 1 : 0;

		}
	}

	public void JammerPlatUpdate(){
		if (destiny == 0 && trade == true) {
			mudarPlatDestino ();
			spr.color = new Color32 (104,0,255,255);
		}
	}
	
}