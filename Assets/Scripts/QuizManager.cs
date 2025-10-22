using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class QuizManager : MonoBehaviour
{
    public static QuizManager Instancia { get; private set; }

    [Header("Referencias de UI")]
    public GameObject panelQuiz;
    public TextMeshProUGUI textoPregunta;
    public Button[] botonesRespuesta = new Button[4];
    public UnityEngine.UI.Image panelFlash;

    [Header("Base de Datos")]
    public List<PreguntaSO> preguntasDeLlave;

    [Header("Feedback")]
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public float duracionFlash = 0.5f;

    private PreguntaSO preguntaActual;
    private AudioSource miAudioSource;

    // --- ¡NUEVAS VARIABLES! ---
    private bool esQuizDeLlave;
    private Checkpoint checkpointPendiente;
    // ---------------------------

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

        miAudioSource = GetComponent<AudioSource>();
        panelQuiz.SetActive(false);

        if (panelFlash != null)
        {
            panelFlash.color = new Color(1, 1, 1, 0);
            panelFlash.gameObject.SetActive(false);
        }
    }

    public void SolicitarQuizDeLlave()
    {
        if (preguntasDeLlave.Count == 0)
        {
            Debug.LogWarning("No hay preguntas de llave asignadas.");
            return;
        }

        esQuizDeLlave = true; // Guardamos que es de Llave
        checkpointPendiente = null;

        Time.timeScale = 0f;
        preguntaActual = preguntasDeLlave[UnityEngine.Random.Range(0, preguntasDeLlave.Count)];

        MostrarPregunta(preguntaActual);
    }

    // --- ¡NUEVO MÉTODO! ---
    public void SolicitarQuizDeCheckpoint(Checkpoint checkpoint)
    {
        // Si el checkpoint ya está activo, no hacemos nada
        if (checkpoint.estaActivado) return;

        // Si no hay preguntas, solo activa el checkpoint y ya
        if (preguntasDeLlave.Count == 0)
        {
            GameManager.Instancia.ActivarNuevoCheckpoint(checkpoint);
            return;
        }

        esQuizDeLlave = false; // Guardamos que es de Checkpoint
        checkpointPendiente = checkpoint;

        Time.timeScale = 0f;
        preguntaActual = preguntasDeLlave[UnityEngine.Random.Range(0, preguntasDeLlave.Count)];

        MostrarPregunta(preguntaActual);
    }
    // ---------------------------

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
        bool esCorrecto = (indiceRespuesta == preguntaActual.indiceRespuestaCorrecta);
        StartCoroutine(RutinaFeedback(esCorrecto));
    }

    private IEnumerator RutinaFeedback(bool esCorrecto)
    {
        // ... (Toda la lógica del flash y sonido se queda igual) ...
        // 1. Fade In
        panelFlash.gameObject.SetActive(true);
        Color colorFlash = esCorrecto ? Color.green : Color.red;
        if (esCorrecto) miAudioSource.PlayOneShot(sonidoCorrecto);
        else miAudioSource.PlayOneShot(sonidoIncorrecto);

        float tiempoPasado = 0f;
        float duracionFade = duracionFlash / 2;

        while (tiempoPasado < duracionFade)
        {
            tiempoPasado += Time.unscaledDeltaTime;
            float alfa = Mathf.Lerp(0, 1, tiempoPasado / duracionFade);
            panelFlash.color = new Color(colorFlash.r, colorFlash.g, colorFlash.b, alfa);
            yield return null;
        }

        tiempoPasado = 0f;

        // 2. Fade Out
        while (tiempoPasado < duracionFade)
        {
            tiempoPasado += Time.unscaledDeltaTime;
            float alfa = Mathf.Lerp(1, 0, tiempoPasado / duracionFade);
            panelFlash.color = new Color(colorFlash.r, colorFlash.g, colorFlash.b, alfa);
            yield return null;
        }

        // 3. Finalizar
        panelFlash.gameObject.SetActive(false);

        // --- ¡LÓGICA MODIFICADA! ---
        if (esQuizDeLlave)
        {
            // Era un quiz de Llave
            if (!esCorrecto)
            {
                GameManager.Instancia.PerderVidaPorQuiz();
            }
        }
        else
        {
            // Era un quiz de Checkpoint
            if (esCorrecto)
            {
                GameManager.Instancia.AumentarVida();
            }

            // Siempre activamos el checkpoint (al acertar o fallar)
            if (checkpointPendiente != null)
            {
                GameManager.Instancia.ActivarNuevoCheckpoint(checkpointPendiente);
                checkpointPendiente = null; // Limpiamos la referencia
            }
        }
        // ---------------------------

        Time.timeScale = 1f; // Reanuda el juego
    }
}