using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Sierra : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform puntoA; 
    public Transform puntoB; 
    public float velocidadMovimiento = 3f;

    [Header("Configuración de Rotación")]
    public float velocidadGiro = 300f; 

    private Transform puntoDestino;

    void Start()
    {
        // Empezar en el punto A
        transform.position = puntoA.position;
        // Poner el primer destino
        puntoDestino = puntoB;
    }

    void Update()
    {
        // --- 1. Rotación Constante ---
        // Gira sobre el eje Z (el eje "profundo" en 2D)
        transform.Rotate(0, 0, velocidadGiro * Time.deltaTime);

        // --- 2. Movimiento de Patrulla ---
        // Moverse suavemente hacia el punto destino
        transform.position = Vector3.MoveTowards(transform.position, puntoDestino.position, velocidadMovimiento * Time.deltaTime);

        // --- 3. Comprobar si llegó al destino ---
        // Si la distancia es muy pequeña, cambia de destino
        if (Vector3.Distance(transform.position, puntoDestino.position) < 0.1f)
        {
            if (puntoDestino == puntoA)
            {
                puntoDestino = puntoB;
            }
            else
            {
                puntoDestino = puntoA;
            }
        }
    }

    // --- (Opcional) Dibuja la ruta en el Editor ---
    // Esto es muy útil para que puedas ver la patrulla
    private void OnDrawGizmos()
    {
        if (puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(puntoA.position, puntoB.position);
            Gizmos.DrawWireSphere(puntoA.position, 0.3f);
            Gizmos.DrawWireSphere(puntoB.position, 0.3f);
        }
    }
}