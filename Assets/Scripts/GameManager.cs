using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System;

[RequireComponent(typeof(AudioSource))] 
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Configuración de Muerte")]
    public GameObject prefabExplosion;
    public AudioClip sonidoExplosion;
    public float retrasoReinicio = 1.0f;

    [Header("Configuracion de Nivel")]
    public int llavesTotales = 4;
    private int llavesActuales;
    public int vidasMaximas = 3;
    private int vidasActuales;
    private float tiempoNivel;

    [Header("Referencias de Escena")]
    public GameObject pelota;
    public Transform posicionInicio;
    public PuertaSalida puertaSalida;

    private Vector3 vectorRespawnActual;
    private Checkpoint checkpointActivo;

    [Header("Paneles UI")]
    public GameObject panelGameOver;
    public GameObject panelNivelCompletado;
    public TextMeshProUGUI textoTiempoNivel;
    public AudioClip sonidoGameOver;
    public AudioClip sonidoNivelCompletado; 

    private AudioSource miAudioSource; 

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

        Time.timeScale = 1f;
        llavesActuales = 0;
        vidasActuales = vidasMaximas;
        tiempoNivel = 0f;

        vectorRespawnActual = posicionInicio.position;

        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelNivelCompletado != null) panelNivelCompletado.SetActive(false);
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.ActualizarVidas(vidasActuales);
            UIManager.Instancia.IniciarLlaves(llavesTotales);
        }
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            tiempoNivel += Time.deltaTime;
        }
    }

    public void PerderJuego(Vector3 posicion)
    {
        AudioSource.PlayClipAtPoint(sonidoExplosion, posicion);
        GameObject explosion = Instantiate(prefabExplosion, posicion, Quaternion.identity);
        Destroy(explosion, 0.5f);

        vidasActuales--;
        if (UIManager.Instancia != null) UIManager.Instancia.ActualizarVidas(vidasActuales);

        if (vidasActuales > 0)
        {
            StartCoroutine(RutinaRespawn());
        }
        else
        {
            MostrarGameOver();
        }
    }

    private IEnumerator RutinaRespawn()
    {
        yield return new WaitForSeconds(retrasoReinicio);

        pelota.transform.position = vectorRespawnActual;
        pelota.SetActive(true);

        Rigidbody2D rb = pelota.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void MostrarGameOver()
    {
        if (sonidoGameOver != null) miAudioSource.PlayOneShot(sonidoGameOver); 
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void JugarOtraVez()
    {
        Time.timeScale = 1f;
        ReiniciarNivel();
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void ReiniciarNivel()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);
    }

    public void RecogerLlave()
    {
        llavesActuales++;
        if (UIManager.Instancia != null) UIManager.Instancia.RemoverIconoLlave();

        if (llavesActuales >= llavesTotales)
        {
            if (puertaSalida != null)
            {
                puertaSalida.AbrirPuerta();
            }
        }
    }

    public void IntentarSalir()
    {
        if (puertaSalida != null && puertaSalida.estaAbierta)
        {
            CompletarNivel();
        }
        else
        {
            int faltantes = llavesTotales - llavesActuales;
            Debug.Log($"¡PUERTA CERRADA! Te faltan {faltantes} llaves.");
        }
    }

    private void CompletarNivel()
    {
        Time.timeScale = 0f;
        pelota.SetActive(false);

        if (sonidoNivelCompletado != null) miAudioSource.PlayOneShot(sonidoNivelCompletado);

        if (panelNivelCompletado != null)
        {
            panelNivelCompletado.SetActive(true);
        }

        if (textoTiempoNivel != null)
        {
            textoTiempoNivel.text = $"Tiempo: {tiempoNivel:F1} sec";
        }
    }

    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
        int proximaEscena = SceneManager.GetActiveScene().buildIndex + 1;

        if (proximaEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximaEscena);
        }
        else
        {
            Debug.Log("¡No hay más niveles! Volviendo al menú.");
            VolverAlMenu();
        }
    }

    public void ActivarNuevoCheckpoint(Checkpoint nuevoCheckpoint)
    {
        if (checkpointActivo != null)
        {
            checkpointActivo.Desactivar();
        }

        checkpointActivo = nuevoCheckpoint;
        checkpointActivo.Activar();

        vectorRespawnActual = nuevoCheckpoint.transform.position;
    }

    public void PerderVidaPorQuiz()
    {
        vidasActuales--;
        Debug.Log($"Vida perdida por quiz! Vidas restantes: {vidasActuales}");
        
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.ActualizarVidas(vidasActuales);
        }

        if (vidasActuales <= 0)
        {
            MostrarGameOver();
        }
    }
}