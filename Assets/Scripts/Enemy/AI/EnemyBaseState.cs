// Name: EnemyBaseState.cs
// Author: Connor Larsen
// Date: 02/07/2022

using UnityEngine;

public abstract class EnemyBaseState
{
    #region Functions
    public abstract void EnterState(EnemyStateMachine enemy);
    public abstract void UpdateState(EnemyStateMachine enemy);
    public abstract void OnCollisionEnter(EnemyStateMachine enemy);
    #endregion
}