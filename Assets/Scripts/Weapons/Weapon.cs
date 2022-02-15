// Name: Weapon.cs
// Author: Connor Larsen
// Date: 02/12/2022

using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Serialized and Public Variables
    [Header("Functional Variables")]
    [SerializeField] GameObject[] weaponPrefab;     // Place all child game objects of the weapon here
    [SerializeField] Collider[] weaponColliders;    // Place all colliders on the weapon here

    [Header("Weapon Stats")]
    [SerializeField] string weaponName;     // Name of the weapon
    [SerializeField] public bool semiAuto;  // Determines if weapon is semi auto or full auto
    [SerializeField] public int maxAmmo;    // Maximum amount of ammo weapon can have in a single clip
    [SerializeField] int gunDamage;         // How much damage the weapon deals
    [SerializeField] int fireRate;          // Fire rate of the weapon (shots per second)
    [SerializeField] float reloadSpeed;     // How fast in seconds the gun takes to reload
    [SerializeField] float hitForce;        // Force applied to any rigidbody hit by the weapon
    [SerializeField] float range;           // How far the gun can shoot

    [Header("Physics")]
    [SerializeField] float throwForce;      // Force applied to weapon when thrown
    [SerializeField] float throwExtraForce; // Extra force applied to the weapon when thrown
    [SerializeField] float rotationForce;   // Rotational force applied to the weapon when thrown

    // Hidden Public Variables
    [HideInInspector] public bool isHeld;       // If the weapon is being held or not
    [HideInInspector] public bool isReloading;  // If the weapon is being reloaded or not
    [HideInInspector] public bool isShooting;   // If the weapon is being shot or not
    [HideInInspector] public int currentAmmo;   // Store how much ammo is left in current magazine
    #endregion

    #region Private Variables
    Rigidbody rb;           // Rigidbody attached to the weapon
    Transform playerCamera; // Reference to the player camera
    int weaponLayer;        // Reference to the value of the "Weapon" layer
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();  // Give the weapon a rigidbody component
        rb.mass = 0.1f;                             // Set the mass of the weapon

        currentAmmo = maxAmmo;  // Start with full mag

        weaponLayer = LayerMask.NameToLayer("Weapon");  // Grab the layer value of the weapon layer
    }

    // Shoot the weapon
    public void ShootWeapon()
    {
        // If weapon is semi auto, not shooting and not reloading...
        if (semiAuto == true && isShooting == false && isReloading == false)
        {
            // If no object is hit by the raycast...
            if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out var hitInfo, range))
            {
                return; // Do nothing
            }

            var rb = hitInfo.transform.GetComponent<Rigidbody>();       // Grab the rigidbody off the hit object
            var health = hitInfo.transform.GetComponent<EnemyHealth>(); // Grab the health off the hit object

            // If hit object has neither rigidbody or health...
            if (rb == null && health == null)
            {
                return; // Do nothing
            }

            rb.velocity += playerCamera.forward * hitForce; // Apply force to hit object
            health.TakeDamage(gunDamage);                   // Apply damage to hit enemy

            StartCoroutine(currentAmmo <= 0 ? ReloadCooldown() : ShootingCooldown());   // Reload if out of ammo, otherwise shooting cooldown
        }
    }

    // Reload the weapon
    public void ReloadWeapon()
    {
        StartCoroutine(ReloadCooldown());   // Reload the weapon
    }

    // Pickup the weapon
    public void PickupWeapon(Transform weaponHandler, Transform playerCam)
    {
        // If current weapon is already being held...
        if (isHeld == true)
        {
            return; // Do nothing
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

        isHeld = true;              // Weapon is held
        playerCamera = playerCam;   // Store reference to the player camera
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

    // Cooldown between shots
    private IEnumerator ShootingCooldown()
    {
        isShooting = true;                              // Currently shooting
        yield return new WaitForSeconds(1f / fireRate); // Limit shots by firerate
        isShooting = false;                             // Done shooting
    }

    // Cooldown for reloading weapon
    private IEnumerator ReloadCooldown()
    {
        isReloading = true;                             // Currently reloading
        yield return new WaitForSeconds(reloadSpeed);   // Wait for reload to complete
        currentAmmo = maxAmmo;                          // Reset ammo
        isReloading = false;                            // Done reloading
    }
    #endregion
}