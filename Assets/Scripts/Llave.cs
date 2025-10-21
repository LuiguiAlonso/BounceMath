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

        haSidoColectada = true;
        miCollider.enabled = false;
        spriteRenderer.color = colorRecogido;

        if (miAudioSource != null)
        {
            miAudioSource.Play();
        }

        GameManager.Instancia.RecogerLlave();
        QuizManager.Instancia.SolicitarQuizDeLlave();
    }
}