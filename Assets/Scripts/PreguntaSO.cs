using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Pregunta", menuName = "Quiz/PreguntaSO")]
public class PreguntaSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string pregunta;

    public string[] alternativas = new string[4];

    [Range(0, 3)]
    public int indiceRespuestaCorrecta;

    [Header("Feedback")]
    [TextArea(3, 10)]
    public string feedbackSolucion; 
}