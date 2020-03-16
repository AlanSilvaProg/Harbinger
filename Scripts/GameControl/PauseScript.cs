using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {





	public Animator pauseAnimator;

	public static List<string> titulos = new List<string>();
	public static List<string> conteudos = new List<string>();

	public Button[] botoesLog = new Button[18];
	public Text[] textsLog = new Text[18];
	public Image[] setasLog = new Image[18];
	public Text textConteudo;
	public Scrollbar scroolTitulos;

	public List<string> tituloss = new List<string>();
	public List<string> conteudoss = new List<string>();

	public Image[] setasSystem = new Image[2];

	private int qualSystem = 0;

	private int qualLoreAgora;
	private int qualAgora; // 0 = map... 1 = system... -1 = log 

	public GameObject warningHud;
	public static GameObject warningHudStatic;
	// Use this for initialization
	void Start () {

		qualAgora = 0;
		qualLoreAgora = 0;

		textConteudo.text = "  ";
		warningHudStatic = warningHud;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Global.pause && warningHudStatic.activeSelf) {
			warningHudStatic.SetActive (false);
		}
		for (int i = 17; i > titulos.Count;) {
			i--;
			botoesLog [i].gameObject.SetActive(false);
		}

		for (int i = 0; i < titulos.Count; i++) {
			botoesLog [i].gameObject.SetActive(true);
			textsLog [i].text = titulos[i];
		}

		if (titulos.Count > tituloss.Count) {
			tituloss.Add (titulos [titulos.Count-1]);
			conteudoss.Add (conteudos [conteudos.Count-1]);
		}

		if (Global.pause && qualAgora == 1) {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				if (qualSystem == 0) {
					qualSystem = 1;
				} else if (qualSystem == 1) {
					qualSystem = 0;
				}
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				if (qualSystem == 0) {
					qualSystem = 1;
				} else if (qualSystem == 1) {
					qualSystem = 0;
				}
			}

			if (qualSystem == 0) {
				setasSystem [0].gameObject.SetActive (true);
				setasSystem [1].gameObject.SetActive (false);
			} else if (qualSystem == 1) {
				setasSystem [0].gameObject.SetActive (false);
				setasSystem [1].gameObject.SetActive (true);
			}

			if (Input.GetKeyDown (KeyCode.Return)) {
				if (qualSystem == 0) {
					gameObject.SendMessage ("Despause");
				} else if (qualSystem == 1) {
					gameObject.SendMessage ("Menu");
				}
			}

		}

		if (Global.pause && qualAgora == -1) {
			if (Input.GetKeyDown (KeyCode.DownArrow) && qualLoreAgora < 17 && (qualLoreAgora + 1) < titulos.Count) {
				if (qualLoreAgora >= 8 && scroolTitulos.value > 0) {
					scroolTitulos.value -= 0.2f;
				}
				qualLoreAgora += 1;
			}
			if (Input.GetKeyDown (KeyCode.UpArrow) && qualLoreAgora > 0) {
				if (qualLoreAgora < 12 && scroolTitulos.value < 1) {
					scroolTitulos.value += 0.2f;
				}
				qualLoreAgora -= 1;
			}
		} else {
			qualLoreAgora = 0;
		}

		if (Global.pause) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (qualAgora == 1) {
					qualAgora = -1;
				} else {
					qualAgora += 1;
				}
				pauseAnimator.SetTrigger ("Next");
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (qualAgora == -1) {
					qualAgora = 1;
				} else {
					qualAgora -= 1;
				}
				pauseAnimator.SetTrigger ("Back");
			}
		}

		switch (qualLoreAgora) {
		case 0:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[0];
			break;
		case 1:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[1];
			break;
		case 2:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[2];
			break;
		case 3:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[3];
			break;
		case 4:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[4];
			break;
		case 5:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[5];
			break;
		case 6:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[6];
			break;
		case 7:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[7];
			break;
		case 8:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[8];
			break;
		case 9:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[9];
			break;
		case 10:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[10];
			break;
		case 11:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[11];
			break;
		case 12:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[12];
			break;
		case 13:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[13];
			break;
		case 14:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[14];
			break;
		case 15:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[15];
			break;
		case 16:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[16];
			break;
		case 17:
			for (int i = 0; i < titulos.Count; i++) {
				if (i != qualLoreAgora) {
					setasLog [i].gameObject.SetActive (false);
				} else if (i == qualLoreAgora) {
					setasLog [i].gameObject.SetActive (true);
				}
			}
			textConteudo.text = conteudos[17];
			break;
		}
	}

}
