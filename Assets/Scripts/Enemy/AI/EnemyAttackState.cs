// Name: EnemyAttackState.cs
// Author: Connor Larsen
// Date: 02/07/2022

using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    #region Functions
    // Enter State Function
    public override void EnterState(EnemyStateMachine enemy)
    {
        Debug.Log("Enemy Entered 'Attack State'");  // DEBUG: Print out a debug message
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        #region Transitions
        // Enemy Loses Sight of Player ==> Idle State

        // Enemy HP Reaches ~10% ==> Flee State

        // Enemy HP Depleted ==> Dead State
        #endregion
    }

    // OnCollisionEnter Function
    public override void OnCollisionEnter(EnemyStateMachine enemy)
    {

    }
    #endregion
}