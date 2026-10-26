using UnityEngine;
using UnityEngine.Video;
using TMPro;


public class TelevisionEasterEgg : MonoBehaviour
{
    
    public PlayerPoints player;
    
    public TextMeshPro worldText;

    
    
    public MeshRenderer screenMeshRenderer;

    
    public VideoClip videoClip;
    public int requiredPoints = 5;

    public bool hideScreenOnPause = false;

    private VideoPlayer videoPlayer;
    private bool isPrepared = false;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (worldText == null)
        {
            worldText = GetComponentInChildren<TextMeshPro>();
        }

        ConfigureVideoPlayer();

        // Ocultar la pantalla inicialmente si está marcada la opción
        if (hideScreenOnPause && screenMeshRenderer != null)
        {
            screenMeshRenderer.enabled = false;
        }
    }

    private void ConfigureVideoPlayer()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.clip = videoClip;

        if (screenMeshRenderer != null)
        {
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.targetMaterialRenderer = screenMeshRenderer;

            Material mat = screenMeshRenderer.sharedMaterial;
            if (mat != null && mat.HasProperty("_BaseMap"))
            {
                videoPlayer.targetMaterialProperty = "_BaseMap";
            }
            else
            {
                videoPlayer.targetMaterialProperty = "_MainTex";
            }
        }
    }

    public void OnPointerEnter()
    {
        if (player == null)
        {
            Debug.LogWarning(name + ": ¡Falta asignar el script PlayerPoints!");
            return;
        }

        if (player.GetPoints() <= requiredPoints)
        {
            ResumeOrPlayVideo();
        }
    }

    public void OnPointerExit()
    {
        PauseVideo();
    }

    private void ResumeOrPlayVideo()
    {
        if (videoPlayer == null || videoClip == null) return;

        // Ocultar el texto de números para mostrar la imagen
        if (worldText != null)
        {
            worldText.enabled = false;
        }

        if (screenMeshRenderer != null)
        {
            screenMeshRenderer.enabled = true;
        }

        // Si es la primera vez que se reproduce
        if (!isPrepared)
        {
            videoPlayer.clip = videoClip;
            isPrepared = true;
        }

        // Reanuda la reproducción desde el frame en que se quedó
        videoPlayer.Play();
    }

    private void PauseVideo()
    {
        // En lugar de videoPlayer.Stop(), usamos Pause() para congelar el frame actual
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }

        // Si quieres que el texto regrese mientras está en pausa:
        if (hideScreenOnPause)
        {
            if (worldText != null)
            {
                worldText.enabled = true;
            }

            if (screenMeshRenderer != null)
            {
                screenMeshRenderer.enabled = false;
            }
        }
        // Si hideScreenOnPause es false, la pantalla se queda visible congelada en ese frame exacto
    }
}