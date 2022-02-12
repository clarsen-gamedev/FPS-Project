// Name: Weapon.cs
// Author: Connor Larsen
// Date: 02/12/2022

using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] Collider[] weaponColliders;
    [SerializeField] float throwForce;
    [SerializeField] float throwExtraForce;
    [SerializeField] float rotationForce;
    #endregion

    #region Private Variables
    Rigidbody rigidbody;
    bool isHeld;
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();   // Give the weapon a rigidbody component
        rigidbody.mass = 0.1f;                              // Set the mass of the weapon
    }

    // Pickup the weapon
    public void PickupWeapon(Transform weaponHandler)
    {
        // If current weapon is already being held...
        if (isHeld == true)
        {
            return; // Leave function
        }

        // Weapon can be equipped
        Destroy(rigidbody); // Destroy the weapon's rigidbody on pickup

        transform.parent = weaponHandler;               // Place the weapon in the weapon handler
        transform.localPosition = Vector3.zero;         // Zero out the weapon's local position
        transform.localRotation = Quaternion.identity;  // Reset weapon rotation

        foreach (Collider c in weaponColliders)  // Disable all colliders on the weapon
        {
            c.enabled = false;
        }
        
        isHeld = true;  // Weapon is held
    }

    // Drop the weapon
    public void DropWeapon(Transform playerCam)
    {
        // If current weapon is not being held...
        if (isHeld == false)
        {
            return; // Leave function
        }

        // Weapon can be dropped
        rigidbody = gameObject.AddComponent<Rigidbody>();   // Give the weapon a rigidbody component
        rigidbody.mass = 0.1f;                              // Set the mass of the weapon

        Vector3 forward = playerCam.forward;                                // Grab the forward vector from the player cam
        forward.y = 0f;                                                     // Set forward y to 0
        rigidbody.velocity = forward * throwForce;                          // Throw the weapon forward
        rigidbody.velocity += Vector3.up * throwExtraForce;                 // Add upwards force to weapon throw
        rigidbody.angularVelocity = Random.onUnitSphere * rotationForce;    // Add rotational force to weapon throw

        foreach (Collider c in weaponColliders)  // Enable all colliders on the weapon
        {
            c.enabled = true;
        }

        transform.parent = null;        // Weapon has no parent
        isHeld = false;                 // Weapon is no longer being held
    }
    #endregion
}