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
    public string weaponName;               // Name of the weapon
    public bool semiAuto;                   // Determines if weapon is semi auto or full auto
    public int maxAmmo;                     // Maximum amount of ammo weapon can have in a single clip
    [SerializeField] int gunDamage;         // How much damage the weapon deals
    [SerializeField] int fireRate;          // Fire rate of the weapon (shots per second)
    [SerializeField] float reloadSpeed;     // How fast in seconds the gun takes to reload
    [SerializeField] float range;           // How far the gun can shoot
    [SerializeField] float shotSpread;      // Spread of the bullets shot
    [SerializeField] float shotCooldown;    // Cooldown between shots (only for full auto weapons)

    [Header("Physics")]
    [SerializeField] float throwForce;      // Force applied to weapon when thrown
    [SerializeField] float throwExtraForce; // Extra force applied to the weapon when thrown
    [SerializeField] float rotationForce;   // Rotational force applied to the weapon when thrown

    // Hidden Public Variables
    [HideInInspector] public bool isHeld;       // If the weapon is being held or not
    [HideInInspector] public bool isReloading;  // If the weapon is being reloaded or not
    [HideInInspector] public bool isShooting;   // If the weapon is being shot or not
    [HideInInspector] public bool triggerHeld;  // If trigger is being held down (only for full auto weapons)
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
        // If weapon is not shooting and not reloading...
        if (isShooting == false && isReloading == false)
        {
            // Shot Spread
            float spreadX = Random.Range(-shotSpread, shotSpread);
            float spreadY = Random.Range(-shotSpread, shotSpread);

            // Calculate Direction with Spread
            Vector3 direction = playerCamera.forward + new Vector3(spreadX, spreadY, 0f);

            // Raycast
            RaycastHit hit; // Initialize a raycast
            if (Physics.Raycast(playerCamera.position, direction, out hit)) // Shoot a raycast
            {
                var health = hit.transform.GetComponent<EnemyHealth>(); // Grab the enemy health script off the object hit

                // If the object hit has an enemy health script...
                if (health != null)
                {
                    health.TakeDamage(gunDamage);   // Apply damage to enemy
                }
            }

            // Shooting Audio

            currentAmmo--;                          // Decrease ammo count
            Invoke("ShootingCooldown", fireRate);   // Wait for cooldown before shooting again

            if (semiAuto) return;   // Exit function if semi auto

            // If weapon is full auto, have bullets left and trigger is still held...
            if (currentAmmo > 0)
            {
                Invoke("ShootWeapon", shotCooldown);    // Shoot again after shot cooldown
            }
        }
    }

    // Reload the weapon
    public void ReloadWeapon()
    {
        // Reload Audio

        // Reload Functionality
        isReloading = true;                     // Currently reloading
        //gunAnimator.SetTrigger("Reload");       // Play the reload animation
        Invoke("ReloadFinished", reloadSpeed);  // DEBUG: Wait for reload to finish
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
    private void ShootingCooldown()
    {
        isShooting = false;                             // Done shooting
    }

    // Call function at the end of reload animation
    private void ReloadFinished()
    {
        currentAmmo = maxAmmo;  // Refill magazine
        isReloading = false;    // Player is done reloading
    }
    #endregion
}