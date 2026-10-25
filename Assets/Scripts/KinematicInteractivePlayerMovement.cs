using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class KinematicInteractivePlayerMovement : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float moveSpeed = 3.5f;
    public float gravity = -9.81f;
    public Transform cameraTransform;

    [Header("Controles")]
    public bool useKeyboardMovement = true;
    public bool useGazeMovement = true;
    public float gazeAngleThreshold = 20.0f;

    [Header("Físicas de Empuje")]
    public float pushPower = 2.0f; // Fuerza con la que empujas objetos

    private CharacterController controller;
    private float verticalVelocity = 0f;

    void Start() 
    {
        controller = GetComponent<CharacterController>();

        if (!cameraTransform && Camera.main != null) 
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update() 
    {
        if (!cameraTransform) return;

        // 1. Orientaciones horizontales de la cámara (plano XZ)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = Vector3.zero;

        // 2. Lectura por Teclado (WASD / Flechas)
        Vector2 keyboardInput = Vector2.zero;
        if (useKeyboardMovement && Keyboard.current != null) 
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) keyboardInput.y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) keyboardInput.y -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) keyboardInput.x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) keyboardInput.x += 1f;
            keyboardInput.Normalize();
        }

        // 3. Lectura del Mando (Gamepad)
        Vector2 stickInput = Vector2.zero;
        if (Gamepad.current != null) 
        {
            stickInput = Gamepad.current.leftStick.ReadValue();
        }

        // Prioridad: Teclado -> Mando -> Mirada (Cardboard)
        if (keyboardInput.sqrMagnitude > 0.05f) 
        {
            targetDirection = forward * keyboardInput.y + right * keyboardInput.x;
        } 
        else if (stickInput.sqrMagnitude > 0.05f) 
        {
            targetDirection = forward * stickInput.y + right * stickInput.x;
        } 
        else if (useGazeMovement) 
        {
            float pitch = cameraTransform.eulerAngles.x;
            if (pitch > 180f) pitch -= 360f;

            if (pitch > gazeAngleThreshold && pitch < 75.0f) 
            {
                targetDirection = forward;
            }
        }

        // 4. Calculamos velocidad horizontal + Gravedad vertical
        Vector3 movement = targetDirection * moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0) 
        {
            verticalVelocity = -2f; // Mantiene al jugador pegado al suelo en bajadas
        }
        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        // 5. Movemos el Character Controller (frena solo contra los muros)
        controller.Move(movement * Time.deltaTime);
    }

    // Este método de Unity se ejecuta automáticamente cuando el CharacterController choca con algo al moverse
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Si el objeto con el que chocamos no tiene Rigidbody o es Kinematic (inamovible), no hacemos nada
        if (body == null || body.isKinematic) 
        {
            return;
        }

        // Evitamos empujar objetos hacia abajo si nos subimos encima de ellos
        if (hit.moveDirection.y < -0.3f) 
        {
            return;
        }

        // Calculamos la dirección horizontal del empujón y aplicamos velocidad al objeto
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * pushPower;
    }
}