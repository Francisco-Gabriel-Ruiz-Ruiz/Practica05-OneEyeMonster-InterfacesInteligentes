using UnityEngine;
using UnityEngine.InputSystem;

public class KinematicPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public Transform cameraTransform;

    public bool useKeyboardMovement = true;
    public bool useGazeMovement = false;
    public float gazeAngleThreshold = 20.0f;

    private Rigidbody myRigidbody;
    private Vector3 calculatedMovement = Vector3.zero;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        if (!myRigidbody || !myRigidbody.isKinematic) {
            Debug.LogError(name + ": ¡No se ha asignado un componente Rigidbody isKinematic a este GameObject!");
        }

        if (!cameraTransform) {
            Debug.LogError(name + ": ¡No se ha asignado el Transform de la cámara principal!");
        }
    }

    void Update() {
        // Obtenemos orientaciones horizontales de la cámara (plano XZ)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = Vector3.zero;

        // 1. Lectura por Teclado (WASD / Flechas)
        Vector2 keyboardInput = Vector2.zero;
        if (useKeyboardMovement && Keyboard.current != null) {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) {
                keyboardInput.y += 1f;
            }
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) {
                keyboardInput.y -= 1f;
            }
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) {
                keyboardInput.x -= 1f;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) {
                keyboardInput.x += 1f;
            }
            keyboardInput.Normalize();
        }

        // 2. Lectura del Input System (Gamepad)
        Vector2 stickInput = Vector2.zero;
        if (Gamepad.current != null) {
            stickInput = Gamepad.current.leftStick.ReadValue();
        }

        // Prioridad: Teclado -> Mando -> Mirada
        if (keyboardInput.sqrMagnitude > 0.05f) {
            targetDirection = forward * keyboardInput.y + right * keyboardInput.x;
        } else if (stickInput.sqrMagnitude > 0.05f) {
            targetDirection = forward * stickInput.y + right * stickInput.x;
        } else if (useGazeMovement) { // 3. Movimiento con la vista (si no se usa teclado ni mando)
            float pitch = cameraTransform.eulerAngles.x;
            if (pitch > 180f) {
                pitch -= 360f;
            }
            if (pitch > gazeAngleThreshold && pitch < 75.0f) {
                targetDirection = forward;
            }
        }
        calculatedMovement = targetDirection * moveSpeed;
    }

    void FixedUpdate() {
        if (calculatedMovement.sqrMagnitude > 0.001f) {
            Vector3 targetPosition = myRigidbody.position + calculatedMovement * Time.fixedDeltaTime;
            myRigidbody.MovePosition(targetPosition);
        }
    }
}