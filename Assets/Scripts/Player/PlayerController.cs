using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController charController;
    private Vector3 moveDirection;
    private Animator animator;
    private float verticalVelocity = 0f;
    private PlayerInput playerInput;
    private InputAction moveAction;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Obtener input del usuario (WASD) usando el nuevo Input System
        Vector2 input = moveAction.ReadValue<Vector2>();
        float inputX = input.x;
        float inputZ = input.y;

        // Crear dirección de movimiento normalizada
        moveDirection = new Vector3(inputX, 0, inputZ).normalized;

        if (moveDirection.magnitude > 0)
        {
            // Rotar hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Aplicar movimiento
            charController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Actualizar animación
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            // Parado - Idle
            animator.SetFloat("Speed", 0f);
        }

        // Aplicar gravedad
        verticalVelocity -= 9.8f * Time.deltaTime;
        charController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    // Método para obtener la dirección actual del jugador
    public Vector3 GetForwardDirection()
    {
        return transform.forward;
    }
}