// Name: EnemyIdleState.cs
// Author: Connor Larsen
// Date: 02/07/2022

using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    #region Private Variables
    Vector3 patrolPoint;            // Stores the point the enemy patrols to
    bool patrolPointSet;            // True if patrol point has been set
    float patrolWaitTimer;          // How long the enemy waits before moving to a new point
    #endregion

    #region Functions
    // Enter State Function
    public override void EnterState(EnemyStateMachine enemy)
    {
        // DEBUG CODE
        Debug.Log("Enemy Entered 'Idle State'");                        // DEBUG: Print out a debug message
        enemy.GetComponent<Renderer>().material.color = Color.yellow;   // DEBUG: Make Idle enemies Yellow

        // Initialize State
        patrolWaitTimer = Random.Range(enemy.minWaitTIme, enemy.maxWaitTime);   // Set the wait timer
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        #region State Behaviour
        // If a patrol point has not been set...
        if (!patrolPointSet)
        {
            patrolWaitTimer -= Time.deltaTime;  // Decrease wait timer

            // If timer runs out...
            if (patrolWaitTimer <= 0)
            {
                FindPatrolPoint(enemy); // Grab a new patrol point
                patrolWaitTimer = 0;    // Reset the timer
            }
        }

        // Patrol point has been set
        else
        {
            enemy.agent.destination = patrolPoint;  // Set new destination for enemy
        }

        Vector3 patrolPointDistance = enemy.transform.position - patrolPoint;   // Store current distance from the patrol point

        // If enemy has reached the patrol point...
        if (patrolPointDistance.magnitude < 1f)
        {
            patrolPointSet = false; // Search for a new patrol point

            // If the timer has not yet been reset...
            if (patrolWaitTimer == 0)
            {
                patrolWaitTimer = Random.Range(enemy.minWaitTIme, enemy.maxWaitTime);   // Set the wait timer
            }
        }
        #endregion

        #region Transitions
        // Enemy Sees Player ==> Attack State
        if (enemy.seePlayer == true)
        {
            enemy.SwitchState(enemy.attackState);   // Switch to the attack state
        }

        // Enemy HP Reaches ~25% ==> Flee State
        if (enemy.health.GetEnemyHealth() <= (enemy.health.GetMaxHealth() / 4))
        {
            enemy.SwitchState(enemy.fleeState);
        }

        // Dead State
        if (enemy.GetComponent<EnemyHealth>().GetEnemyHealth() <= 0)
        {
            enemy.SwitchState(enemy.deadState); // Switch to the dead state
        }
        #endregion
    }

    // Search for a new patrol point
    private void FindPatrolPoint(EnemyStateMachine enemy)
    {
        float posX = Random.Range(-enemy.patrolRange, enemy.patrolRange);   // Get random X coordinate
        float posZ = Random.Range(-enemy.patrolRange, enemy.patrolRange);   // Get random Z coordinate

        patrolPoint = new Vector3(enemy.transform.position.x + posX, enemy.transform.position.y, enemy.transform.position.z + posZ);    // Store the new patrol point

        // Check to see if new patrol point is on the walkable floor...
        if (Physics.Raycast(patrolPoint, -enemy.transform.up, 2f, enemy.floorMask))
        {
            patrolPointSet = true;  // New patrol point has been set
        }
    }
    #endregion
}