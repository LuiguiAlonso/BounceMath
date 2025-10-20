using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; 
using System;  

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Configuracion de Muerte")]
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

    [Header("Paneles UI")]
    public GameObject panelGameOver;
    public GameObject panelNivelCompletado; 
    public TextMeshProUGUI textoTiempoNivel;  

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

        Time.timeScale = 1f;
        llavesActuales = 0;
        vidasActuales = vidasMaximas;
        tiempoNivel = 0f;

        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelNivelCompletado != null) panelNivelCompletado.SetActive(false);
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
        Instantiate(prefabExplosion, posicion, Quaternion.identity);

        vidasActuales--;
        Debug.Log($"Vida perdida! Vidas restantes: {vidasActuales}");

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

        pelota.transform.position = posicionInicio.position;
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
        Debug.Log($"Llave recogida! Tienes: {llavesActuales} / {llavesTotales}");

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

        if (panelNivelCompletado != null)
        {
            panelNivelCompletado.SetActive(true);
        }

        if (textoTiempoNivel != null)
        {
            TimeSpan t = TimeSpan.FromSeconds(tiempoNivel);
            string tiempoFormateado = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            textoTiempoNivel.text = $"Tiempo: {tiempoFormateado}";
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
            Debug.Log("Volviendo al menu.");
            VolverAlMenu();
        }
    }
}