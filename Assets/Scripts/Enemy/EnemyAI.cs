using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectDistance = 15f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackDamage = 15f;

    private float lastAttackTime = 0f;
    private CharacterController charController;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private bool isDead = false;
    private float verticalVelocity = 0f;

    // Estados del AI
    private enum AIState { Idle, Chase, Attack, Dead }
    private AIState currentState = AIState.Idle;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

        // Buscar el jugador automáticamente si no está asignado
        if (playerTransform == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                playerTransform = playerGO.transform;
                Debug.Log("Jugador encontrado automáticamente");
            }
            else
            {
                Debug.LogError("No se encontró al jugador. Por favor asigna la referencia en el Inspector.");
            }
        }
    }

    void Update()
    {
        if (playerTransform == null || isDead)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Cambiar estado basado en distancia
        if (distanceToPlayer < attackDistance)
        {
            currentState = AIState.Attack;
        }
        else if (distanceToPlayer < detectDistance)
        {
            currentState = AIState.Chase;
        }
        else
        {
            currentState = AIState.Idle;
        }

        // Ejecutar comportamiento según el estado
        switch (currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Chase:
                ChasePlayer(distanceToPlayer);
                break;
            case AIState.Attack:
                AttackPlayer();
                break;
            case AIState.Dead:
                break;
        }
    }

    void Idle()
    {
        // No hacer nada, solo estar de pie
        animator.SetFloat("Speed", 0f);
    }

    void ChasePlayer(float distanceToPlayer)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Rotar hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        // Moverse hacia el jugador
        charController.Move(directionToPlayer * moveSpeed * Time.deltaTime);

        // Aplicar gravedad
        verticalVelocity -= 9.8f * Time.deltaTime;
        charController.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        // Animar movimiento
        animator.SetFloat("Speed", 1f);

        Debug.Log("Enemigo persiguiendo. Distancia: " + distanceToPlayer.ToString("F2"));
    }

    void AttackPlayer()
    {
        // Verificar cooldown de ataque
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        // Reproducir animación de ataque
        animator.SetTrigger("Attack");

        Debug.Log("¡El enemigo ataca!");

        // Aplicar daño al jugador
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("¡El enemigo golpeó al jugador! Daño: " + attackDamage);
        }

        animator.SetFloat("Speed", 0f);
    }

    /// <summary>
    /// Método llamado cuando el enemigo recibe daño
    /// </summary>
    public void OnEnemyHit()
    {
        // Reproducir animación de recibir golpe
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    /// <summary>
    /// Método llamado cuando el enemigo muere
    /// </summary>
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        currentState = AIState.Dead;
        Debug.Log("¡Enemigo derrotado!");

        // Reproducir animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        // Desactivar movimiento
        if (charController != null)
        {
            charController.enabled = false;
        }

        // Desactivar este script
        this.enabled = false;

        // Destruir el objeto después de 2 segundos
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// Retorna si el enemigo está vivo
    /// </summary>
    public bool IsAlive()
    {
        return !isDead;
    }

    // Visualizar rango de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}