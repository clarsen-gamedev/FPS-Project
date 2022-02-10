// Name: EnemyStateMachine.cs
// Author: Connor Larsen
// Date: 02/01/2022

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    #region Public Variables
    [Header("AI Movement")]
    public NavMeshAgent agent;      // Reference to the enemy nav mesh agent

    [Header("AI Field of View")]
    [HideInInspector] public GameObject playerRef;  // Reference to the player character in the scene
    public LayerMask targetMask;                    // Reference to the layer mask of the target
    public LayerMask obstacleMask;                  // Reference to the layer mask of obstructions to the FOV
    [HideInInspector] public bool seePlayer;        // Enemy can see the player
    public float radius;                            // Radius of the enemy's field of view
    [Range(0, 360)] public float angle;             // Angle of the enemy's field of view

    // All Possible States for the Enemy
    public EnemyIdleState idleState = new EnemyIdleState();        // Idle State
    public EnemyAttackState attackState = new EnemyAttackState();  // Attack State
    public EnemyFleeState fleeState = new EnemyFleeState();        // Flee State
    public EnemyDeadState deadState = new EnemyDeadState();        // Dead State
    #endregion

    #region Private Variables
    EnemyBaseState currentState;    // Stores reference to the active state of the enemy
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        currentState = idleState;                               // Initialize the starting state for the enemy (Idle)
        playerRef = GameObject.FindGameObjectWithTag("Player"); // Find the player and store the reference

        StartCoroutine(LookForPlayer());    // Start looking for the player
        currentState.EnterState(this);      // Run the "EnterState" function attached to the current state
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this); // Run the "Update" function based on which state is currently active
    }

    // Switch State Function
    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;   // Switch to the new state
        state.EnterState(this); // Run the "EnterState" function attached to the switched state
    }

    // Constantly look for the player
    public IEnumerator LookForPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); // Set delay time for coroutine

        while (true)
        {
            yield return wait;  // Wait for delay time to pass
            FieldOfViewCheck(); // Check for the player
        }
    }

    // Check for the player in the enemy's FOV
    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask); // Check the FOV radius for the player and store all hits in an array

        // If a target has been found...
        if (rangeChecks.Length > 0)
        {
            Transform target = rangeChecks[0].transform;                                    // Store the transform of the player
            Vector3 directionToTarget = (target.position - transform.position).normalized;  // Create a normalized Vector3 in the direction of the player

            // If the player is within the FOV angle...
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position); // Store the distance from the enemy to the player

                // If there is no obstacle blocking line of sight...
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    seePlayer = true;   // See player
                }

                // An obstacle is blocking line of sight
                else
                {
                    seePlayer = false;  // Can't see player
                }
            }

            // Player is not within the FOV angle
            else
            {
                seePlayer = false;  // Can't see player
            }
        }

        // If no target is found...
        else if (seePlayer)
        {
            seePlayer = false;  // Can't see player
        }
    }
    #endregion
}