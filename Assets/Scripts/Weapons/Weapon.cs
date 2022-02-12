// Name: Weapon.cs
// Author: Connor Larsen
// Date: 02/12/2022

using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] Collider weaponCollider;
    [SerializeField] float throwForce;
    [SerializeField] float throwExtraForce;
    [SerializeField] float rotationForce;
    #endregion
}