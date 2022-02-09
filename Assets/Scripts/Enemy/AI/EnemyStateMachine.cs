// Name: EnemyStateMachine.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;
//using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    #region Public Variables
    [Header("AI Movement")]
    //public NavMeshAgent agent;        // Reference to the enemy nav mesh agent
    public Rigidbody rigidbody;         // Reference to the enemy rigidbody
    public float moveSpeedIdle = 20f;   // How fast the AI moves around while idle
    public float rotationSpeed = 100f;  // How fast the AI rotates
    
    [Header("Bools")]
    public bool isWandering = false;        // If the AI is wandering or not
    public bool isRotatingLeft = false;     // If the AI is rotating to the left
    public bool isRotatingRight = false;    // If the AI is rotating to the right
    public bool isWalking = false;          // If the AI is currently walking

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
        currentState = idleState;       // Initialize the starting state for the enemy (Idle)
        currentState.EnterState(this);  // Run the "EnterState" function attached to the current state
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
    #endregion
}