using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteApagado;
    public Sprite spriteEncendido;

    [Header("Sonido")]
    public AudioClip sonidoActivacion;

    private SpriteRenderer spriteRenderer;
    private AudioSource miAudioSource;
    public bool estaActivado { get; private set; } = false;

    private bool haSidoActivadoAlgunaVez = false; // ¡NUEVA VARIABLE!

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteApagado;

        miAudioSource = GetComponent<AudioSource>();
        miAudioSource.playOnAwake = false;
    }

    public void Activar()
    {
        if (estaActivado) return; // Si ya es el checkpoint activo, no hace nada

        estaActivado = true;
        spriteRenderer.sprite = spriteEncendido;

        // --- ¡LÓGICA MODIFICADA! ---
        // Solo reproduce el sonido si es la PRIMERA VEZ que se activa
        if (!haSidoActivadoAlgunaVez)
        {
            if (miAudioSource != null && sonidoActivacion != null)
            {
                miAudioSource.PlayOneShot(sonidoActivacion);
            }

            // Marca que ya se usó una vez
            haSidoActivadoAlgunaVez = true;
        }
    }

    public void Desactivar()
    {
        estaActivado = false;
        spriteRenderer.sprite = spriteApagado;
    }
}