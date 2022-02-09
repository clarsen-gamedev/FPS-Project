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
        // DEBUG CODE
        Debug.Log("Enemy Entered 'Idle State'");                        // DEBUG: Print out a debug message
        enemy.GetComponent<Renderer>().material.color = Color.yellow;   // DEBUG: Make Idle enemies Yellow

        // Reset bools
        enemy.isWandering = false;
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        #region State Behaviour
        // If the enemy isn't currently wandering...
        if (enemy.isWandering == false)
        {
            
        }
        #endregion

        #region Transitions
        // Enemy Sees Player ==> Attack State

        // Enemy HP Reaches ~10% ==> Flee State

        // Dead State
        if (enemy.GetComponent<EnemyHealth>().GetEnemyHealth() <= 0)
        {
            enemy.SwitchState(enemy.deadState); // Switch to the dead state
        }
        #endregion
    }

    // OnCollisionEnter Function
    public override void OnCollisionEnter(EnemyStateMachine enemy)
    {

    }
    #endregion
}