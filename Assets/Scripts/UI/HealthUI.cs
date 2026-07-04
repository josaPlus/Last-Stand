using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyHealth enemyHealth;

    // Colores para las barras
    private Color healthyColor = Color.green;
    private Color warningColor = Color.yellow;
    private Color criticalColor = Color.red;

    void Start()
    {
        // Buscar referencias automáticamente si no están asignadas
        if (playerHealth == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                playerHealth = playerGO.GetComponent<PlayerHealth>();
                Debug.Log("PlayerHealth encontrado automáticamente");
            }
            else
            {
                Debug.LogError("No se encontró PlayerHealth");
            }
        }

        if (enemyHealth == null)
        {
            GameObject enemyGO = GameObject.Find("Enemy");
            if (enemyGO != null)
            {
                enemyHealth = enemyGO.GetComponent<EnemyHealth>();
                Debug.Log("EnemyHealth encontrado automáticamente");
            }
            else
            {
                Debug.LogError("No se encontró EnemyHealth. Verifica que el enemigo se llame 'Enemy'");
            }
        }

        if (playerHealthBar == null)
        {
            Debug.LogError("PlayerHealthBar no está asignada en HealthUI");
        }

        if (enemyHealthBar == null)
        {
            Debug.LogError("EnemyHealthBar no está asignada en HealthUI");
        }
    }

    void Update()
    {
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {
        // Actualizar barra de vida del jugador
        if (playerHealth != null && playerHealthBar != null)
        {
            float playerHealthPercent = playerHealth.GetHealth() / playerHealth.GetMaxHealth();
            playerHealthBar.fillAmount = Mathf.Clamp01(playerHealthPercent);
            playerHealthBar.color = GetHealthColor(playerHealthPercent);
        }

        // Actualizar barra de vida del enemigo
        if (enemyHealth != null && enemyHealthBar != null)
        {
            float enemyHealthPercent = enemyHealth.GetHealth() / enemyHealth.GetMaxHealth();
            enemyHealthBar.fillAmount = Mathf.Clamp01(enemyHealthPercent);
            enemyHealthBar.color = GetHealthColor(enemyHealthPercent);
        }
    }

    /// <summary>
    /// Retorna el color de la barra según el porcentaje de salud
    /// </summary>
    Color GetHealthColor(float healthPercent)
    {
        if (healthPercent > 0.5f)
            return healthyColor; // Verde
        else if (healthPercent > 0.25f)
            return warningColor; // Amarillo
        else
            return criticalColor; // Rojo
    }
}