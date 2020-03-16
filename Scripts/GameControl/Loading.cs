using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
	[Header("Nome da cena a ser carregada")]
	public static string cenaACarregar;
	[Space(20)]
	public Texture texturaFundos;
	public Texture barraDeProgresso;
	public string textoLoad = "Progresso do carregamento:";
	public Color corDoTexto = Color.white;
	public Font Fonte;
	[Space(20)]
	[Range(0.5f,3.0f)]
	public float tamanhoDoTexto = 1.5f;
	[Range(1,10)]
	public int larguraDaBarra = 8;
	[Range(1,10)]
	public int alturaDaBarra = 2;
	[Range(-4.5f,4.5f)]
	public float deslocarBarra = 4;
	[Range(-8,4)]
	public float deslocarTextoX = 2;
	[Range(-4.5f,4.5f)]
	public float deslocarTextoY = 3;
	private bool avancarCena = true;
	public static bool load;

	private bool mostrarCarregamento = false, menuclick;
	private int progresso = 0;

	void Update () {

		if (load && avancarCena) {
			load = false;
			StartCoroutine(CenaDeCarregamento(cenaACarregar));
		}

		if (avancarCena == true && menuclick) {
			StartCoroutine (CenaDeCarregamento (cenaACarregar));
			menuclick = false;
		}


	}

	IEnumerator CenaDeCarregamento (string cena){
		
		if (SceneManager.GetActiveScene ().name != "MainMenu") {
			Global.PlayerAcess.transform.parent = null;
			print ("unparenting player");
		}
		avancarCena = false;
		mostrarCarregamento = true;
		AsyncOperation carregamento = SceneManager.LoadSceneAsync (cena);
		while (!carregamento.isDone) {
			progresso = (int)(carregamento.progress*100);
			yield return null;
		}

		if (cena == "MainMenu") {
			Destroy(Global.GCAcess.gameObject);
			Destroy(Global.PlayerAcess.gameObject);
			Destroy(Global.cameraa);
			Global.primeiroSum = true;
		}
		if (Global.loadMorte) {
			Global.life = 100;
			Global.Energia = 100;
			Global.armaEquipada = PlayerPrefs.GetInt ("Equipado");
			Global.municao [0] = PlayerPrefs.GetInt ("Municao0");
			Global.municao [1] = PlayerPrefs.GetInt ("Municao1");
			Global.municao [2] = PlayerPrefs.GetInt ("Municao2");
			Global.ultimoOlhar = PlayerPrefs.GetString ("UltimoOlhar");
			Global.PlayerAcess.transform.parent = null;
			Global.PlayerAcess.transform.position = Global.lastPositionToLoad;
			Global.loadMorte = false;
			Global.PlayerAcess.morte = false;
		}

		mostrarCarregamento = false;
		load = false;
		avancarCena = true;

	}

	IEnumerator reativar(){
		yield return new WaitForSeconds (1);
		load = false;
		avancarCena = true;
	}

	void reativ(){
		StartCoroutine ("reativar");
	}

	void OnGUI (){
		if (mostrarCarregamento == true) {
			GUI.contentColor = corDoTexto;
			GUI.skin.font = Fonte;
			GUI.skin.label.fontSize = (int)(Screen.height/50*tamanhoDoTexto);
			//TEXTURA DE FUNDO
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), texturaFundos);

			//TEXTO DE CARREGAMENTO
			float deslocXText = (Screen.height/10)*deslocarTextoX;
			float deslocYText = (Screen.height/10)*deslocarTextoY;
			GUI.Label(new Rect(Screen.width/2 + deslocXText, Screen.height/2 + deslocYText, Screen.width, Screen.height),textoLoad + " " + progresso + "%");  

			//BARRA DE PROGRESSO
			float largura = Screen.width*(larguraDaBarra/10.0f);
			float altura = Screen.height/50*alturaDaBarra;
			float deslocYBar = (Screen.height/10)*deslocarBarra;
			GUI.DrawTexture(new Rect(Screen.width/2 - largura/2, Screen.height/2 - (altura/2) + deslocYBar, largura*(progresso/100.0f), altura), barraDeProgresso);                                    
		}
	}

	void MenuClick(){
		menuclick = true;
	}
}