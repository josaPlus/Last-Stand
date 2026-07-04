using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;

    private float currentHealth;
    private bool isDead = false;
    private EnemyAI enemyAI;

    void Start()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI>();
        Debug.Log("Salud del enemigo inicializada: " + currentHealth);
    }

    /// <summary>
    /// Aplica daño al enemigo
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log("¡El enemigo recibió " + damage + " de daño! Salud actual: " + currentHealth);

        // Reproducir animación de recibir daño
        if (enemyAI != null)
        {
            enemyAI.OnEnemyHit();
        }

        // Comprobar si murió
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Maneja la muerte del enemigo
    /// </summary>
    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("¡Enemigo derrotado!");

        // Llamar a la IA para ejecutar la muerte
        if (enemyAI != null && enemyAI.IsAlive())
        {
            enemyAI.Die();
        }
    }

    /// <summary>
    /// Obtiene la salud actual del enemigo
    /// </summary>
    public float GetHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Obtiene la salud máxima del enemigo
    /// </summary>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Obtiene si el enemigo está vivo
    /// </summary>
    public bool IsAlive()
    {
        return !isDead;
    }
}