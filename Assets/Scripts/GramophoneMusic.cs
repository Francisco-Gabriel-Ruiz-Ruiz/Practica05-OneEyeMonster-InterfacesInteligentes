using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GramophoneMusic : MonoBehaviour
{
    public PlayerPoints player;
    public int rewardForCollecting = 1;
    
    [Header("Configuración de Audio")]
    public AudioClip gramophoneClip;
    private AudioSource audioSource;

    [Header("Rotación en Eje Y")]
    [Tooltip("Velocidad en grados por segundo")]
    public float rotationSpeed = 90f; 
    [Tooltip("Marcar si solo debe girar mientras la música esté sonando")]
    public bool rotateOnlyWhenPlaying = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();

        if (!player) 
        {
            Debug.LogError(name + ": ¡No hay script PlayerPoints asignado!");
        }

        if (gramophoneClip == null)
        {
            Debug.LogWarning(name + ": ¡No hay AudioClip asignado en el inspector!");
        }
    }

    void Update()
    {
        // Si rotateOnlyWhenPlaying está activo, solo rota si el audio se está reproduciendo
        if (rotateOnlyWhenPlaying && (audioSource == null || !audioSource.isPlaying))
        {
            return;
        }

        // Rota sobre su propio eje Y local multiplicando por deltaTime para que sea fluido
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnPointerEnter() 
    {
        if (player != null)
        {
            player.AddPoints(rewardForCollecting);
        }

        PlayMusic();
    }

    private void PlayMusic()
    {
        if (audioSource != null && gramophoneClip != null)
        {
            audioSource.clip = gramophoneClip;
            audioSource.Play();
        }
    }
}