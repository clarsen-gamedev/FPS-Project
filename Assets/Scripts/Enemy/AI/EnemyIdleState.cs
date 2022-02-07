// Name: EnemyIdleState.cs
// Author: Connor Larsen
// Date: 02/07/2022

using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    #region Functions
    // Enter State Function
    public override void EnterState(EnemyStateMachine enemy)
    {
        Debug.Log("Enemy Entered 'Idle State'");    // DEBUG: Print out a debug message
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        #region Transitions
        // Enemy Sees Player ==> Attack State

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