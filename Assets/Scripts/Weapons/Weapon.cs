// Name: Weapon.cs
// Author: Connor Larsen
// Date: 02/12/2022

using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Serialized Variables
    [Header("Functional Variables")]
    [SerializeField] GameObject[] weaponPrefab;     // Place all child game objects of the weapon here
    [SerializeField] Collider[] weaponColliders;    // Place all colliders on the weapon here

    [Header("Weapon Stats")]
    [SerializeField] string weaponName; // Name of the weapon
    [SerializeField] int maxAmmo;       // Maximum amount of ammo weapon can have in a single clip
    [SerializeField] int fireRate;      // Fire rate of the weapon (shots per second)
    [SerializeField] float reloadSpeed; // How fast in seconds the gun takes to reload
    [SerializeField] float hitForce;    // Force applied to any rigidbody hit by the weapon
    [SerializeField] float range;       // How far the gun can shoot

    [Header("Physics")]
    [SerializeField] float throwForce;      // Force applied to weapon when thrown
    [SerializeField] float throwExtraForce; // Extra force applied to the weapon when thrown
    [SerializeField] float rotationForce;   // Rotational force applied to the weapon when thrown
    #endregion

    #region Private Variables
    Rigidbody rb;
    bool isHeld;
    int weaponLayer;
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();  // Give the weapon a rigidbody component
        rb.mass = 0.1f;                             // Set the mass of the weapon

        weaponLayer = LayerMask.NameToLayer("Weapon");  // Grab the layer value of the weapon layer
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
        Destroy(rb); // Destroy the weapon's rigidbody on pickup

        transform.parent = weaponHandler;               // Place the weapon in the weapon handler
        transform.localPosition = Vector3.zero;         // Zero out the weapon's local position
        transform.localRotation = Quaternion.identity;  // Reset weapon rotation

        foreach (Collider c in weaponColliders)  // Disable all colliders on the weapon
        {
            c.enabled = false;
        }

        foreach (GameObject g in weaponPrefab)  // Apply the weapon layer to all attached game objects of the weapon prefab
        {
            g.layer = weaponLayer;
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
        rb = gameObject.AddComponent<Rigidbody>();   // Give the weapon a rigidbody component
        rb.mass = 0.1f;                              // Set the mass of the weapon

        Vector3 forward = playerCam.forward;                                // Grab the forward vector from the player cam
        forward.y = 0f;                                                     // Set forward y to 0
        rb.velocity = forward * throwForce;                          // Throw the weapon forward
        rb.velocity += Vector3.up * throwExtraForce;                 // Add upwards force to weapon throw
        rb.angularVelocity = Random.onUnitSphere * rotationForce;    // Add rotational force to weapon throw

        foreach (Collider c in weaponColliders)  // Enable all colliders on the weapon
        {
            c.enabled = true;
        }

        foreach (GameObject g in weaponPrefab)  // Remove the weapon layer from all attached game objects of the weapon prefab
        {
            g.layer = 0;
        }

        transform.parent = null;        // Weapon has no parent
        isHeld = false;                 // Weapon is no longer being held
    }
    #endregion
}