// Name: EnemyHealth.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] Image healthBarImage;  // Reference to image used for enemy health UI
    [SerializeField] float maxHealth = 100; // Maximum amount of health the enemy can have
    #endregion

    #region Private Variables
    float currentHealth;    // Stores the current health of the enemy
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;  // Set currentHealth equal to maxHealth on script startup
    }

    // Controls how the object attached takes damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;                                // Subtract incoming damage from enemy health
        healthBarImage.fillAmount = currentHealth / maxHealth;  // Calculate the percentage of fill amount for health UI
    }

    // Grab the health value for use in other scripts
    public float GetEnemyHealth()
    {
        return currentHealth;
    }

    // Grab the maximum value of health the enemy can have
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    // Kills the enemy this script is attached to
    public void Die()
    {
        Destroy(gameObject);    // Destroy the enemy game object
    }
    #endregion
}