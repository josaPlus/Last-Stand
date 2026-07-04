using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;
    private PlayerInput playerInput;

    void Start()
    {
        currentHealth = maxHealth;
        playerInput = GetComponent<PlayerInput>();
        Debug.Log("Salud del jugador inicializada: " + currentHealth);
    }

    /// <summary>
    /// Aplica daño al jugador
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log("¡El jugador recibió " + damage + " de daño! Salud actual: " + currentHealth);

        // Reproducir animación de recibir daño (cuando esté disponible)
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador
    /// </summary>
    void Die()
    {
        isDead = true;
        Debug.Log("¡EL JUGADOR HA MUERTO! GAME OVER");

        // Reproducir animación de muerte
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        // Desactivar controles
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        CombatSystem combatSystem = GetComponent<CombatSystem>();
        if (combatSystem != null)
        {
            combatSystem.enabled = false;
        }

        // Desactivar input
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

        // Pausar el juego después de 1 segundo
        Invoke("PauseGame", 1f);
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Obtiene la salud actual del jugador
    /// </summary>
    public float GetHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Obtiene la salud máxima del jugador
    /// </summary>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Obtiene si el jugador está muerto
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }
}