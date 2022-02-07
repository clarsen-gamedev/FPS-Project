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
        // Dead; Do Nothing

        Debug.Log("Enemy Entered 'Dead State'");    // DEBUG: Print out a debug message
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        // Dead; Do Nothing
    }

    // OnCollisionEnter Function
    public override void OnCollisionEnter(EnemyStateMachine enemy)
    {
        // Dead; Do Nothing
    }
    #endregion
}