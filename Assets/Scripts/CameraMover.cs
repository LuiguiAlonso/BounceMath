using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    [Header("Configuraci�n del Movimiento")]
    public Transform posicionMenu;
    public Transform posicionSelector;
    public float duracionMovimiento = 0.7f;
    public AnimationCurve curvaEase;

    private Camera camaraPrincipal;
    private Coroutine corrutinaMovimiento;

    void Start()
    {
        camaraPrincipal = Camera.main;

        if (posicionMenu != null)
        {
            camaraPrincipal.transform.position = posicionMenu.position;
        }
    }

    public void MoverASelector()
    {
        MoverCamara(posicionSelector.position);
    }

    public void MoverAMenu()
    {
        MoverCamara(posicionMenu.position);
    }

    private void MoverCamara(Vector3 destino)
    {
        if (corrutinaMovimiento != null)
        {
            StopCoroutine(corrutinaMovimiento);
        }

        corrutinaMovimiento = StartCoroutine(RutinaMover(destino));
    }

    private IEnumerator RutinaMover(Vector3 destino)
    {
        Vector3 inicio = camaraPrincipal.transform.position;
        float tiempoPasado = 0f;

        while (tiempoPasado < duracionMovimiento)
        {
            tiempoPasado += Time.deltaTime;

            float t = tiempoPasado / duracionMovimiento;

            float tSuavizado = curvaEase.Evaluate(t);

            camaraPrincipal.transform.position = Vector3.Lerp(inicio, destino, tSuavizado);

            yield return null; 
        }

        camaraPrincipal.transform.position = destino;
        corrutinaMovimiento = null;
    }
}