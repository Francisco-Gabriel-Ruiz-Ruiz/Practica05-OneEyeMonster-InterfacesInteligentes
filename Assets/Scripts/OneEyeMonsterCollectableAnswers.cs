using UnityEngine;

public class OneEyeMonsterCollectableAnswers : MonoBehaviour
{
    public float speed = 1;

    public GameObject playerToChase;

    public TelevisionNotificator televisionNotificator;
    public TitanOneEyeMonsterNotificator titanNotificator;
    public PlayerPoints player;
    public OneEyeMonsterReward oneEyeMonsterReward;

    private Renderer[] visualRenderers;
    private Collider[] colliders;
    private Light[] lights;

    private Rigidbody myRigidbody;
    private Vector3 moveDirection;

    private bool isObjectVisible = false;
    private bool isTitanSubscribed = false;
    private bool moveTowardsPlayerFlag = false;

    void Awake() {
        visualRenderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
        lights = GetComponentsInChildren<Light>(true);
        SetVisibility(false);
    }

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        if (!myRigidbody || !myRigidbody.isKinematic) {
            Debug.LogError(name + ": ¡No se ha asignado un Rigidbody isKinematic a este GameObject!");
        }

        if (speed <= 0f) {
            Debug.LogWarning(name + ": La velocidad es menor o igual que cero.");
        }

        if (!playerToChase) {
            Debug.LogError(name + ": ¡¡No se ha asignado un player a perseguir a este GameObject!!");
        }

        if (gameObject.layer != LayerMask.NameToLayer("Interactive")) {
            Debug.LogError(name + ": ¡No está asignado el layer Interactive al ONE EYE MONSTER!");
        }

        if (televisionNotificator != null) {
            televisionNotificator.OnGaze += Activate;
        } else {
            Debug.LogError(name + ": ¡No está asignado el script de la TV notificadora!");
        }
    }

    void OnDestroy() {
        UnsubscribeAll();
    }

    public bool GetIsObjectVisible() {
        return isObjectVisible;
    }

    void Activate() {
        Debug.Log(name + ": Me he activado.");
        SetVisibility(true);

        // Al hacerse visible, nos suscribimos al titán para la recolección automática
        if (!isTitanSubscribed && titanNotificator != null) {
            titanNotificator.OnGaze += MoveTowardsPlayer;
            isTitanSubscribed = true;
            Debug.Log(name + ": Suscrito al titán tras activarse.");
        }
    }

    void MoveTowardsPlayer() {
        Debug.Log(name + ": Voy a comenzar a perseguir al jugador.");
        moveTowardsPlayerFlag = true;
    }

    void Update() {
        moveDirection = Vector3.zero;
        if (moveTowardsPlayerFlag) {
            Vector3 direction = playerToChase.transform.position - transform.position;
            moveDirection = direction.normalized * speed;
        }
    }

    void FixedUpdate() {
        if (!moveTowardsPlayerFlag) {
            return;
        }
        Vector3 finalMoveDirection = moveDirection * Time.fixedDeltaTime;
        myRigidbody.MovePosition(myRigidbody.position + finalMoveDirection);
    }

  private void UnsubscribeAll() {
        if (televisionNotificator != null) {
            televisionNotificator.OnGaze -= Activate;
        }

        if (titanNotificator != null && isTitanSubscribed) {
            titanNotificator.OnGaze -= MoveTowardsPlayer;
            isTitanSubscribed = false;
        }
    }

    private void SetVisibility(bool state) {
        isObjectVisible = state;
        Debug.Log(name + ": Ejecutando SetVisibility = " + state);

        if (visualRenderers != null) {
            foreach (var r in visualRenderers) {
                if (r != null) {
                    r.enabled = state;
                }
            }
        }
        if (colliders != null) {
            foreach (var col in colliders) {
                if (col != null) {
                    col.enabled = state;
                }
            }
        }
        if (lights != null) {
            foreach (var lightComp in lights) {
                if (lightComp != null) {
                    lightComp.enabled = state;
                }
            }
        }
    }
}