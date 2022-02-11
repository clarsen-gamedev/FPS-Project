// Name: EnemyIdleState.cs
// Author: Connor Larsen
// Date: 02/07/2022

using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    #region Private Variables
    GameObject[] patrolPoints;  // Array of enemy patrol points
    int point;                  // Store the index of the next patrol point
    bool isMoving;              // Keep track if enemy is walking or not
    #endregion

    #region Functions
    // Enter State Function
    public override void EnterState(EnemyStateMachine enemy)
    {
        // DEBUG CODE
        Debug.Log("Enemy Entered 'Idle State'");                        // DEBUG: Print out a debug message
        enemy.GetComponent<Renderer>().material.color = Color.yellow;   // DEBUG: Make Idle enemies Yellow

        // Initialize State
        patrolPoints = GameObject.FindGameObjectsWithTag("Waypoint");   // Grab all waypoints in the scene
        point = Random.Range(0, patrolPoints.Length);                   // Pick a random patrol point
    }

    // Update State Function
    public override void UpdateState(EnemyStateMachine enemy)
    {
        #region State Behaviour
        // If enemy has reached the target point...
        if (Vector3.Distance(enemy.transform.position, patrolPoints[point].transform.position) <= 1.0f)
        {
            point = Random.Range(0, patrolPoints.Length);   // Pick a random patrol point
        }
        
        // If enemy still hasn't reached their destination
        else
        {
            enemy.agent.destination = patrolPoints[point].transform.position;   // Move towards the target waypoint
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

    // OnCollisionEnter Function
    public override void OnCollisionEnter(EnemyStateMachine enemy)
    {

    }
    #endregion
}