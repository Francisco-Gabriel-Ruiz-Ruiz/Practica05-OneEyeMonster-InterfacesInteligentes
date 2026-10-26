using UnityEngine;


public class GramophoneMusic : MonoBehaviour
{
    public PlayerPoints player;
    public int rewardForCollecting = 1;
    
    public AudioClip gramophoneClip;
    private AudioSource audioSource;

    public AudioClip easterEggClip;
    public int easterEggPointsThreshold = 10;

    public float rotationSpeed = 90f; 
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
            Debug.LogWarning(name + ": ¡No hay AudioClip estándar asignado en el inspector!");
        }
    }

    void Update()
    {
        if (rotateOnlyWhenPlaying && (audioSource == null || !audioSource.isPlaying))
        {
            return;
        }

        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnPointerEnter() 
    {
        PlayMusic();
        
        if (player != null)
        {
            player.AddPoints(rewardForCollecting);
        }
    }

    void OnPointerExit() 
    {
        
    }

    private void PlayMusic()
    {
        if (audioSource == null) return;

        // Selección por defecto: música normal
        AudioClip clipToPlay = gramophoneClip;

        // Si el jugador cumple o supera el umbral y hay clip asignado, se reproduce el easter egg
        // Nota: Asegúrate de que tu script PlayerPoints tenga la variable o método público para leer los puntos
        // (ej. 'player.points' o 'player.currentPoints' o 'player.GetPoints()')
        if (player != null && player.GetPoints() <= easterEggPointsThreshold && easterEggClip != null)
        {
            clipToPlay = easterEggClip;
        }

        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }
}