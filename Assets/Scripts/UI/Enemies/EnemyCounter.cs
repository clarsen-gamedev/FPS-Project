// Name: EnemyCounter.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    // Serialized Variables
    [SerializeField] Text counterUI;

    // Private Variables
    bool allEnemiesKilled;
    int numEnemies;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        numEnemies = 0;
        allEnemiesKilled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  // Grab all enemies currently in the scene and add them to an array
        numEnemies = enemies.Length;                                        // Set numEnemies to length of the array

        if (enemies.Length == 0)    // Once all enemies are killed...
        {
            allEnemiesKilled = true;    // Set allEnemiesKilled to true
        }

        counterUI.text = "Enemies: " + numEnemies;  // Update the UI
    }
}