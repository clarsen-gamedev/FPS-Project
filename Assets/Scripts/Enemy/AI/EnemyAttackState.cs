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
        Debug.Log("Enemy Entered 'Attack State'");                  // DEBUG: Print out a debug message
        enemy.GetComponent<Renderer>().material.color = Color.red;  // DEBUG: Make Idle enemies Red
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        #region Transitions
        // Enemy Loses Sight of Player ==> Idle State

        // Enemy HP Reaches ~10% ==> Flee State

        // Enemy HP Depleted ==> Dead State
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