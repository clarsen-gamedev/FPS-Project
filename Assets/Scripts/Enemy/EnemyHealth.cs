// Name: EnemyHealth.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Public Variables
    public float health = 100f;

    // Controls how the object attached takes damage
    public void TakeDamage(float damage)
    {
        health -= damage;   // Subtract incoming damage from enemy health

        if (health <= 0f)   // Once the enemy is out of health
        {
            Die();  // Kill the enemy
        }
    }

    // Kills the enemy this script is attached to
    void Die()
    {
        Destroy(gameObject);    // Destroy the enemy game object
    }
}