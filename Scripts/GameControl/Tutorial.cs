using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	[Header("Acessos Auxiliares da UI")]
	public Text dialogoText;
	public GameObject caixaDeDialogo;
	[Header("Atualização De Texto Que Será Apresentado")]
	public string textoAtual;
	[Header("Velocidade do Texto, Quanto Maior, Mais Rápido")]
	public float LPS;

	private bool tuto = false, triggerTuto = false;
	private float contadorTempo;
	private int l;
	private int tutoAtual, limiteTuto = 3;

	// Use this for initialization
	void Start () {

		dialogoText.text = "   ";
		Global.tutoAtual = 0;
		tutoAtual = Global.tutoAtual;

	}

	// Update is called once per frame
	void Update () {

		//print (Global.tutoAtual);

//Atualizando tutoAtual local
		tutoAtual = Global.tutoAtual;

//Acessando os arquivos de texto com as escrituras do tutorial
		switch(tutoAtual){
		case 0:
			textoAtual = Global.tuto[0];
			break;
		case 1:
			textoAtual = Global.tuto[1];
			break;
		case 2:
			textoAtual = Global.tuto[2];
			break;
		case 3:
			textoAtual = Global.tuto[3];
			break;
		}


		//Fazendo contagem de tempo para cada atualização
		contadorTempo += Time.deltaTime;

		// Ativando e Desativando caixa de dialogo
		if (tuto == false) {
			caixaDeDialogo.SetActive (false);
			l = 0;
			dialogoText.text = "   ";
		} else {
			caixaDeDialogo.SetActive (true);
		}

		//Atualizando texto atual

		if(Input.GetKeyDown("e") && tuto == true){
			if (l >= textoAtual.Length - 1) {
				if (tutoAtual >= limiteTuto) {
					tuto = false;
					tutoAtual = 0;
				} else {
					Global.tutoAtual++;
					l = 0;
					dialogoText.text = "   ";
				}
			} else {
				while (l < textoAtual.Length - 1) {
					dialogoText.text += textoAtual [l];
					l++;
				}
			}
		}

		if(tuto == true)
			lerTexto();

	}

	void lerTexto(){

		if (contadorTempo > (1f / LPS) && l < textoAtual.Length - 1) {
			dialogoText.text += textoAtual [l];
			l++;
			contadorTempo = 0;
		}
	}

// Função de trigger do tutorial
	public void tutoTrigger(Vector2 textRef){ // X do vetor é a referencia numérica do texto no vetor de tuto no global e y a quantidade de tutoriais que serão apresentados em sequência
		Global.tutoAtual = (int)textRef.x;
		limiteTuto = (int)(textRef.x+textRef.y);
		l = 0;
		dialogoText.text = "   ";
		tuto = true;
	}

}