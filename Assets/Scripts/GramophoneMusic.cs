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
    }

    void Update() {
        if (rotateOnlyWhenPlaying && (!audioSource || !audioSource.isPlaying)) {
            return;
        }
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnPointerEnter() {
        PlayMusic();
        if (player != null) {
            player.AddPoints(rewardForCollecting);
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
        // Si el jugador cumple o supera el umbral y hay clip asignado, se reproduce el easter egg
        if (player != null && player.GetPoints() <= easterEggPointsThreshold && easterEggClip != null) {
            clipToPlay = easterEggClip;
        }
        if (clipToPlay != null) {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }
}