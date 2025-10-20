using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
    public GameObject prefabExplosion;
    public AudioClip sonidoExplosion; 
    public float retrasoReinicio = 1.0f;

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

    public void PerderJuego(Vector3 posicion)
    {
        AudioSource.PlayClipAtPoint(sonidoExplosion, posicion);
        Instantiate(prefabExplosion, posicion, Quaternion.identity);
        StartCoroutine(RutinaReiniciarNivel());
    }

    private IEnumerator RutinaReiniciarNivel()
    {
        yield return new WaitForSeconds(retrasoReinicio);
        ReiniciarNivel();
    }

    private void ReiniciarNivel()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);
    }
}