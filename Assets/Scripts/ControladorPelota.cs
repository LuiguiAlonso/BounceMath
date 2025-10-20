using UnityEngine;

public class ControladorPelota : MonoBehaviour
{
    public float velocidad = 6f;
    public float salto = 8f;
    public LayerMask capaSuelo;
    public float distanciaRaycast = 0.6f;
    private Rigidbody2D rb;
    private bool estaEnSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Salto();
    }

    void FixedUpdate()
    {
        TocaSuelo();
        Movimiento();
    }

    void Salto()
    {
        if (estaEnSuelo && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * salto, ForceMode2D.Impulse);
        }
    }

    void Movimiento()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputHorizontal * velocidad, rb.linearVelocity.y);
    }

    void TocaSuelo()
    {
        estaEnSuelo = Physics2D.Raycast(transform.position, Vector2.down, distanciaRaycast, capaSuelo);
        Color colorRayo = estaEnSuelo ? Color.green : Color.red;
        Debug.DrawRay(transform.position, Vector2.down * distanciaRaycast, colorRayo);
    }
}