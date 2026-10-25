using UnityEngine;
using UnityEngine.Video;
using TMPro;

[RequireComponent(typeof(VideoPlayer))]
public class TelevisionEasterEgg : MonoBehaviour
{
    [Header("Referencias del Jugador y Texto")]
    public PlayerPoints player;
    [Tooltip("El texto 3D que muestra los puntos o el mensaje inicial")]
    public TextMeshPro worldText;

    [Header("Pantalla Hija (Cubo)")]
    [Tooltip("Arrastra aquí el cubo 3D que creaste como hijo")]
    public MeshRenderer screenMeshRenderer;

    [Header("Configuración del Vídeo")]
    public VideoClip videoClip;
    public int requiredPoints = 5;

    [Header("Comportamiento")]
    [Tooltip("¿Ocultar o apagar el cubo de la pantalla hasta que empiece el vídeo?")]
    public bool hideScreenUntilPlay = true;
    [Tooltip("Si se desmarca, el vídeo no se corta al apartar la mirada")]
    public bool stopOnPointerExit = false;

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Si no se asignó en el inspector, intenta buscar el TextMeshPro en los hijos
        if (worldText == null)
        {
            worldText = GetComponentInChildren<TextMeshPro>();
        }

        // Configuración inicial del VideoPlayer hacia el MeshRenderer del cubo
        ConfigureVideoPlayer();

        // Ocultar la pantalla inicialmente si está marcada la opción
        if (hideScreenUntilPlay && screenMeshRenderer != null)
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
            // Detecta automáticamente la propiedad de textura del material del cubo
            // (funciona tanto para Standard _MainTex como para URP _BaseMap)
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

        // Comprueba los puntos con el método GetPoints()
        if (player.GetPoints() >= requiredPoints)
        {
            PlayScreenVideo();
        }
    }

    public void OnPointerExit()
    {
        if (stopOnPointerExit)
        {
            StopScreenVideo();
        }
    }

    private void PlayScreenVideo()
    {
        if (videoPlayer == null || videoClip == null) return;

        // Ocultar el texto de números para que no estorbe
        if (worldText != null)
        {
            worldText.enabled = false;
        }

        // Mostrar el cubo de la pantalla
        if (screenMeshRenderer != null)
        {
            screenMeshRenderer.enabled = true;
        }

        videoPlayer.clip = videoClip;
        videoPlayer.Play();
    }

    private void StopScreenVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        // Restaurar texto y ocultar pantalla
        if (worldText != null)
        {
            worldText.enabled = true;
        }

        if (hideScreenUntilPlay && screenMeshRenderer != null)
        {
            screenMeshRenderer.enabled = false;
        }
    }
}