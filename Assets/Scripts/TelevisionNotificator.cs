using System.Collections;
using UnityEngine;

public class TelevisionNotificator : MonoBehaviour
{
    public delegate void message();
    public event message OnGaze;
    public float tiempoRequerido = 1f;
    
    private Coroutine gazeCoroutine;
    private bool hasGazedOnce;

    void Start() {
        if (gameObject.layer != LayerMask.NameToLayer("Interactive")) {
            Debug.LogError(name + ": ¡No está asignado el layer Interactive a la TV notificadora!");
        }
        hasGazedOnce = false;
    }

    public void OnPointerEnter() {
        // Si ya había una corrutina activa por seguridad la detenemos antes de iniciar otra
        if (gazeCoroutine != null) {
            StopCoroutine(gazeCoroutine);
        }
        if (!hasGazedOnce) {
            gazeCoroutine = StartCoroutine(ContadorGaze());
        }
    }

    public void OnPointerExit() {
        // Si aparta la mirada antes de los 3 segundos, cancelamos el temporizador
        if (gazeCoroutine != null) {
            StopCoroutine(gazeCoroutine);
            gazeCoroutine = null;
            Debug.Log(name + ": Mirada interrumpida antes de tiempo.");
        }
    }

    private IEnumerator ContadorGaze() {
        Debug.Log(name + ": Mirada detectada. Esperando " + tiempoRequerido + " segundos...");
        // Espera los 3 segundos de tiempo de juego
        yield return new WaitForSeconds(tiempoRequerido);
        Debug.Log(name + ": Mandando mensaje OnGaze()...");
        // Invocación segura del evento verificando que tenga suscriptores
        OnGaze();
        hasGazedOnce = true;
        // Limpiamos la referencia al finalizar
        gazeCoroutine = null;
    }
}