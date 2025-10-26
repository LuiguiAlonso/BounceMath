using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class Llave : MonoBehaviour
{
    public Color colorRecogido = Color.black;
    public AudioClip sonidoRecogido;

    private SpriteRenderer spriteRenderer;
    private Collider2D miCollider;
    private AudioSource miAudioSource;
    private bool haSidoColectada = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        miCollider = GetComponent<Collider2D>();

        if (sonidoRecogido != null)
        {
            miAudioSource = gameObject.AddComponent<AudioSource>();
            miAudioSource.clip = sonidoRecogido;
            miAudioSource.playOnAwake = false;
        }
    }

    public void Colectar()
    {
        if (haSidoColectada) return;

        // Ya no se auto-recolecta
        // Pide al QuizManager que inicie el quiz,
        // pasándose a sí misma como referencia.
        QuizManager.Instancia.SolicitarQuizDeLlave(this);
    }

    // --- ¡NUEVO MÉTODO PÚBLICO! ---
    // El QuizManager llamará a esto SOLO si la respuesta es correcta.
    public void CompletarRecoleccion()
    {
        if (haSidoColectada) return;

        haSidoColectada = true;
        miCollider.enabled = false;
        spriteRenderer.color = colorRecogido;

        if (miAudioSource != null)
        {
            miAudioSource.Play();
        }

        // Avisa al GameManager (para que actualice la UI de llaves)
        GameManager.Instancia.RecogerLlave();
    }
}