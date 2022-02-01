// Name: EnemyStateMachine.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    // Private Variables
    GameObject[] pointList;     // Array of patrol points
    Transform playerLocation;   // Location of the player character
    Vector3 destPos;            // Next destination for the enemy
    float shootRate;            // Rate of enemy shooting
    float elapsedTime;          // 
}