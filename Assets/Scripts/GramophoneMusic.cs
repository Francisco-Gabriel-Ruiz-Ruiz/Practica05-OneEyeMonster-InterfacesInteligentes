using UnityEngine;

public class GramophoneMusic : MonoBehaviour
{
    public int easterEggPointsThreshold = 0;
    public float rotationSpeed = 90f;
    public bool rotateOnlyWhenPlaying = false;

    public PlayerPoints player;
    public int rewardForCollecting = 1;
    public AudioClip gramophoneClip;
    public AudioClip easterEggClip;

    private AudioSource audioSource;
    private bool alreadyGivenPoints;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) {
            Debug.LogError(name + ": ¡Este GameObject no tiene un componente AudioSource!");
        }
        if (!player) {
            Debug.LogError(name + ": ¡No hay script PlayerPoints asignado!");
        }
        if (!gramophoneClip) {
            Debug.LogWarning(name + ": ¡No hay AudioClip estándar asignado en el inspector!");
        }
        if (!easterEggClip) {
            Debug.LogWarning(name + ": ¡No hay AudioClip Easter Egg asignado en el inspector!");
        }
        alreadyGivenPoints = false;
    }

    void Update() {
        if (rotateOnlyWhenPlaying && (!audioSource || !audioSource.isPlaying)) {
            return;
        }
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnPointerEnter() {
        PlayMusic();
        if (!alreadyGivenPoints && player != null) {
            player.TakePoints(rewardForCollecting);
            alreadyGivenPoints = true;
        }
    }

    void OnPointerExit() {
        // Do nothing.
    }

    private void PlayMusic() {
        if (!audioSource) {
            return;
        }

        AudioClip clipToPlay = gramophoneClip;
            // Determinar si aplica el Easter Egg
        bool shouldPlayEasterEgg = (player != null) && (player.GetPoints() <= easterEggPointsThreshold) && (easterEggClip != null);
        if (shouldPlayEasterEgg) {
            clipToPlay = easterEggClip;
        }

        if (!clipToPlay) {
            return;
        }

        // Si ya está sonando algo
        if (audioSource.isPlaying) {
            // Si el que va a sonar es el Easter Egg, pero ya estaba sonando el Easter Egg, no reiniciar
            if (shouldPlayEasterEgg && audioSource.clip == easterEggClip) {
                return;
            }

            // Si NO es Easter Egg (es música normal), no sobreescribir lo que esté sonando
            if (!shouldPlayEasterEgg) {
                return;
            }
        }

        // Se asigna y reproduce (solo se ejecuta si no estaba sonando nada, o si el Easter Egg interrumpe la música estándar)
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}