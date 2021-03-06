// Name: EnemyDeadState.cs
// Author: Connor Larsen
// Date: 02/07/2022

using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    #region Functions
    // Enter State Function
    public override void EnterState(EnemyStateMachine enemy)
    {
        Debug.Log("Enemy Entered 'Dead State'");    // DEBUG: Print out a debug message
        enemy.GetComponent<EnemyHealth>().Die();    // Kill the enemy
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        // Dead; Do Nothing
    }
    #endregion
}