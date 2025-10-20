using UnityEngine;
using TMPro;
using System.Collections.Generic; 

public class UIManager : MonoBehaviour
{
    public static UIManager Instancia { get; private set; }

    [Header("Componentes de Vidas")]
    public TextMeshProUGUI textoVidas;

    [Header("Componentes de Llaves")]
    public List<GameObject> iconosLlave;

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
    }

    public void IniciarLlaves(int cantidad)
    {
        for (int i = 0; i < iconosLlave.Count; i++)
        {
            iconosLlave[i].SetActive(i < cantidad);
        }
    }

    public void ActualizarVidas(int vidas)
    {
        if (textoVidas != null)
        {
            textoVidas.text = $"x{vidas}";
        }
    }

    public void RemoverIconoLlave()
    {
        for (int i = iconosLlave.Count - 1; i >= 0; i--)
        {
            if (iconosLlave[i].activeSelf)
            {
                iconosLlave[i].SetActive(false);
                break; 
            }
        }
    }
}