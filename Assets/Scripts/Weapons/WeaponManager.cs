// Name: WeaponManager.cs
// Author: Connor Larsen
// Date: 02/12/2022

// https://www.youtube.com/watch?v=QUgQe1K9fH0 Pickup at 43:20

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] Transform weaponHolder;    // Reference to the position of the weapon holder
    [SerializeField] Transform playerCam;       // Reference to the position of the player camera
    [SerializeField] float pickupRange;         // Range weapons can be picked up from
    [SerializeField] float pickupRadius;        // Radius weapons can be picked up from
    #endregion

    #region Private Variables
    Weapon heldWeapon;      // Reference to the currently held weapon
    bool weaponIsHeld;      // If weapon is held
    int weaponLayer;   // Int value of the weapon layer
    #endregion

    #region Functions
    // Awake runs on script initalization
    private void Awake()
    {
        weaponLayer = LayerMask.NameToLayer("Weapon"); // Grab the layer value of the weapon layer
    }

    // Shoot a weapon
    public void Shoot(InputAction.CallbackContext context)
    {
        // If the button has been pressed and weapon is being held...
        if (context.performed && heldWeapon.isHeld == true)
        {
            heldWeapon.ShootWeapon();   // Shoot the weapon
        }
    }

    // Reload a weapon
    public void Reload(InputAction.CallbackContext context)
    {
        // If the button has been pressed, weapon isn't currently reloading and ammo is less than max...
        if (context.performed && heldWeapon.isReloading == false && heldWeapon.currentAmmo < heldWeapon.maxAmmo)
        {
            heldWeapon.ReloadWeapon();  // Reload the weapon
        }
    }

    // Pickup a weapon
    public void Pickup(InputAction.CallbackContext context)
    {
        // If the button has been pressed and no weapon is being held...
        if (context.performed && weaponIsHeld == false)
        {
            var hitList = new RaycastHit[256];                                                                                                                                  // Create a raycast hit array
            var hitNumber = Physics.CapsuleCastNonAlloc(playerCam.position, playerCam.position + playerCam.forward * pickupRange, pickupRadius, playerCam.forward, hitList);    // Grab all objects within pickup range of the player

            var realList = new List<RaycastHit>();  // Create a list for all hits in the raycast
            for (int i = 0; i < hitNumber; i++)
            {
                var hit = hitList[i];   // Store reference to current hit

                // If the current hit is not part of the weapon layer...
                if (hit.transform.gameObject.layer != weaponLayer)
                {
                    continue;   // Do nothing
                }

                // If the weapon can be clearly seen...
                if (hit.point == Vector3.zero)
                {
                    realList.Add(hit);  // Add hit to the real list
                }

                // Else if the weapon is able to be seen...
                else if (Physics.Raycast(playerCam.position, hit.point - playerCam.position, out var hitInfo, hit.distance + 0.1f) && hitInfo.transform == hit.transform)
                {
                    realList.Add(hit);  // Add hit to the real list
                }

                // If real list is empty...
                if (realList.Count == 0)
                {
                    return; // Do nothing
                }

                // Sort the list so that closest objects to the player are listed first
                realList.Sort((hit1, hit2) =>
                {
                    var dist1 = GetDistance(hit1);
                    var dist2 = GetDistance(hit2);
                    return Mathf.Abs(dist1 - dist2) < 0.001f ? 0 : dist1 < dist2 ? -1 : 1;
                });

                weaponIsHeld = true;                                        // Weapon is held
                heldWeapon = realList[0].transform.GetComponent<Weapon>();  // Grab reference to weapon that was picked up
                heldWeapon.PickupWeapon(weaponHolder, playerCam);           // Call the pickup function on the weapon
            }
        }
    }

    // Drop the currently equipped weapon
    public void Drop(InputAction.CallbackContext context)
    {
        // If the button has been pressed and weapon is currently being held...
        if (context.performed && weaponIsHeld == true)
        {
            heldWeapon.DropWeapon(playerCam);   // Drop the weapon
            heldWeapon = null;                  // Remove reference to dropped weapon
            weaponIsHeld = false;               // No weapon being held
        }
    }

    // Get the distance between two objects
    private float GetDistance(RaycastHit hit)
    {
        return Vector3.Distance(playerCam.position, hit.point == Vector3.zero ? hit.transform.position : hit.point);
    }
    #endregion
}