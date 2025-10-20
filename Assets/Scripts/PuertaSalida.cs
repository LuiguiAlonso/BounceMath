using UnityEngine;

public class PuertaSalida : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteCerrado;
    public Sprite spriteAbierto;

    [HideInInspector]
    public bool estaAbierta = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteCerrado;
        }
    }

    public void AbrirPuerta()
    {
        estaAbierta = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteAbierto;
        }
    }
}

