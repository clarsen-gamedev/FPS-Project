// Name: WeaponSwap.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    #region Public Variables
    public int selectedWeapon = 0;
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon(); // Make sure player spawns with a weapon in hand
    }

    // Function called when player switches weapons
    public GameObject SelectWeapon()
    {
        GameObject temp = null; // Temp variable for storing the selected weapon

        int i = 0;                              // Initialize the index for a foreach loop
        foreach (Transform weapon in transform) // Foreach loop goes through each weapon attached to the player
        {
            if (i == selectedWeapon)                // Check to see if the index matches the value of the selected weapon
            {
                weapon.gameObject.SetActive(true);  // Enable the selected weapon
                temp = weapon.gameObject;           // Store the selected weapon in the temp variable

                weapon.gameObject.GetComponent<GunSystem>().UpdateWeaponUI(weapon.gameObject.GetComponent<GunSystem>().gunName);    // Update the UI name
            }
            else
            {
                weapon.gameObject.SetActive(false); // Disable all other weapons
            }

            i++;    // Increase index by one
        }

        return temp;    // Return the selected weapon
    }
    #endregion
}