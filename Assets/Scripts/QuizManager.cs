using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instancia { get; private set; }

    [Header("Referencias de UI")]
    public GameObject panelQuiz;
    public TextMeshProUGUI textoPregunta;
    public Button[] botonesRespuesta = new Button[4];

    [Header("Base de Datos")]
    public List<PreguntaSO> preguntasDeLlave;

    private PreguntaSO preguntaActual;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instancia = this;
        }

        panelQuiz.SetActive(false);
    }

    public void SolicitarQuizDeLlave()
    {
        if (preguntasDeLlave.Count == 0)
        {
            Debug.LogWarning("No hay preguntas de llave asignadas en el QuizManager.");
            return;
        }

        Time.timeScale = 0f;
        preguntaActual = preguntasDeLlave[UnityEngine.Random.Range(0, preguntasDeLlave.Count)];

        MostrarPregunta(preguntaActual);
    }

    private void MostrarPregunta(PreguntaSO datos)
    {
        textoPregunta.text = datos.pregunta;

        for (int i = 0; i < botonesRespuesta.Length; i++)
        {
            TextMeshProUGUI textoBoton = botonesRespuesta[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textoBoton != null)
            {
                textoBoton.text = datos.alternativas[i];
            }
        }

        panelQuiz.SetActive(true);
    }

    public void Responder(int indiceRespuesta)
    {
        panelQuiz.SetActive(false);
        Time.timeScale = 1f;

        if (indiceRespuesta != preguntaActual.indiceRespuestaCorrecta)
        {
            GameManager.Instancia.PerderVidaPorQuiz();
        }
        else
        {
        }
    }
}