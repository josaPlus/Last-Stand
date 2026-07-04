using UnityEngine;
using UnityEngine.InputSystem;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private float attackDamage = 20f;

    private float lastAttackTime = 0f;
    private Animator animator;
    private PlayerController playerController;
    private PlayerInput playerInput;
    private InputAction attackAction;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];

        // Suscribirse al evento de ataque
        attackAction.performed += OnAttackPerformed;
    }

    void OnAttackPerformed(InputAction.CallbackContext context)
    {
        TryAttack();
    }

    void TryAttack()
    {
        // Verificar si está en cooldown
        if (Time.time - lastAttackTime < attackCooldown)
        {
            Debug.Log("Ataque en cooldown. Espera " + (attackCooldown - (Time.time - lastAttackTime)).ToString("F2") + " segundos");
            return;
        }

        // Registrar tiempo del último ataque
        lastAttackTime = Time.time;

        // Reproducir animación de ataque
        animator.SetTrigger("Attack");

        Debug.Log("¡ATACANDO!");

        // Detectar enemigos en rango
        DetectAndDamageEnemies();
    }

    void DetectAndDamageEnemies()
    {
        // Posición del ataque: adelante del jugador
        Vector3 attackPosition = transform.position + transform.forward * attackRange;

        // Detectar todos los colliders en la esfera de ataque
        Collider[] enemiesHit = Physics.OverlapSphere(attackPosition, attackRadius);

        // Aplicar daño a cada enemigo detectado
        foreach (Collider collider in enemiesHit)
        {
            // Ignorar al jugador mismo
            if (collider.CompareTag("Player"))
                continue;

            // Buscar el script de salud del enemigo
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("¡Golpe certero! Daño aplicado: " + attackDamage);
            }
        }
    }

    // Visualizar el rango de ataque en el editor (opcional pero útil)
    void OnDrawGizmosSelected()
    {
        Vector3 attackPosition = transform.position + transform.forward * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }

    void OnDestroy()
    {
        // Desuscribirse del evento
        if (attackAction != null)
        {
            attackAction.performed -= OnAttackPerformed;
        }
    }
}