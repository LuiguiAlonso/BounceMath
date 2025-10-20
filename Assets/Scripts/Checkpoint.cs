using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteApagado;
    public Sprite spriteEncendido;

    private SpriteRenderer spriteRenderer;
    private bool estaActivado = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteApagado;
    }

    public void Activar()
    {
        if (estaActivado) return;

        estaActivado = true;
        spriteRenderer.sprite = spriteEncendido;

    }

    public void Desactivar()
    {
        estaActivado = false;
        spriteRenderer.sprite = spriteApagado;
    }
}