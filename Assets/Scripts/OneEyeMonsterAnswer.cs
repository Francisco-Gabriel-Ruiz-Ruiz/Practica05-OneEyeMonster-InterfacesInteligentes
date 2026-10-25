using UnityEngine;

public class OneEyeMonsterAnswer : MonoBehaviour
{
    public TelevisionNotificator televisionNotificator;

    private Renderer[] visualRenderers;
    private Collider[] colliders;

    void Awake() {
        visualRenderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
        SetVisibility(false);
    }

    void Start() {
        if (gameObject.layer != LayerMask.NameToLayer("Interactive")) {
            Debug.LogError(name + ": ¡No está asignado el layer Interactive al ONE EYE MONSTER!");
        }
        if (!televisionNotificator) {
            Debug.LogError(name + ": ¡No está asignado el script de la TV notificadora!");
        } else {
            televisionNotificator.OnGaze += Activate;
        }
    }

    void OnDestroy() { // Ahorramos memoria
        if (!televisionNotificator) {
            televisionNotificator.OnGaze -= Activate;
        }
    }

    void Activate() {
        Debug.Log(name + ": Me he activado.");
        SetVisibility(true);
    }

    private void SetVisibility(bool state) {
        Debug.Log("ejecutando setvisibiliy=" + state);
        if (visualRenderers != null) {
            foreach (var renderer in visualRenderers) {
                if (renderer != null) {
                    renderer.enabled = state;
                }
            }
        }
        if (colliders != null) {
            foreach (var collider in colliders) {
                if (collider != null) {
                    collider.enabled = state;
                }
            }
        }
    }
}