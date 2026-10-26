using UnityEngine;

public class OneEyeMonsterTelevisionAnswer : MonoBehaviour
{
    public TelevisionNotificator televisionNotificator;

    private Renderer[] visualRenderers;
    private Collider[] colliders;

    private bool isObjectVisible = false;

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

    public bool GetIsObjectVisible() {
        return isObjectVisible;
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
        isObjectVisible = state;
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