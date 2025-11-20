using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class LevelButton : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public string nombreEscena; 
    public bool desbloqueado = false;

    [Header("Referencias de UI")]
    public UnityEngine.UI.Image[] estrellas;
    public Color colorEstrellaGanada = Color.white;
    public Color colorEstrellaPerdida = Color.black;

    private Button miBoton;

    void Awake()
    {
        miBoton = GetComponent<Button>();
    }

    void Start()
    {
        miBoton = GetComponent<Button>();
        ActualizarVisual();

        miBoton.onClick.AddListener(CargarNivel);
    }



    public void ActualizarVisual()
    {
        if (!desbloqueado)
        {
            miBoton.interactable = false;
        }
        else
        {
            miBoton.interactable = true;
            int estrellasGuardadas = DataManager.Instancia.GetEstrellas(nombreEscena);

            for (int i = 0; i < estrellas.Length; i++)
            {
                if (i < estrellasGuardadas)
                {
                    estrellas[i].color = colorEstrellaGanada;
                }
                else
                {
                    estrellas[i].color = colorEstrellaPerdida;
                }
            }
        }
    }

    void CargarNivel()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }

    void OnEnable()
    {
        if (DataManager.Instancia != null)
        {
            ActualizarVisual();
        }
    }
}