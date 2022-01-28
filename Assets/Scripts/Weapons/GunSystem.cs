using UnityEngine;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    // Public Variables
    [Header("Animation")]
    [SerializeField] Animator gunAnimator;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject impactEffect;

    [Header("Sound Effects")]
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip reloadSound;

    [Header("UI Elements")]
    [SerializeField] Text equippedWeapon;
    [SerializeField] Text ammoCounter;
    
    [Header("Gun Stats")]
    [SerializeField] string gunName;
    [SerializeField] int gunDamage;
    [SerializeField] float timeBetweenShooting;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int magSize;
    [SerializeField] int shotsPerTap;
    public bool allowTriggerHold;

    [Header("Reference Variables")]
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera playerCamera;
    [SerializeField] AudioSource audioSource;

    // Private Variables
    bool isShooting;
    bool readyToShoot;
    bool isReloading;
    int bulletsLeft;
    int bulletsShot;

    // Awake is called on script launch
    private void Awake()
    {
        equippedWeapon.text = gunName;  // Place the name of the weapon next to the ammo counter on the UI
        bulletsLeft = magSize;          // Start with gun fully loaded
        readyToShoot = true;            // Start with gun fireable
    }

    // Update is called once per frame
    private void Update()
    {
        isShooting = playerController.GetIsShooting();
        GunControls();  // Perform gun controls
        ammoCounter.text = bulletsLeft + "/" + magSize;
    }

    private void GunControls()
    {
        if (allowTriggerHold)   // Check to see if attack key can be held
        {
            if (bulletsLeft > 0)
            {
                gunAnimator.SetBool("isShooting", isShooting);    // Loop the shooting animation
            }
            else
            {
                gunAnimator.SetBool("isShooting", false);   // Stop the shooting animation
            }
        }

        // Shoot Gun
        if (readyToShoot && isShooting && !isReloading && bulletsLeft > 0)  // Check if player can shoot, if they're shooting, if they're reloading and have bullets left
        {
            // Weapon Function
            bulletsShot = shotsPerTap;
            Shoot();

            // Animations
            if (allowTriggerHold == false)
            {
                gunAnimator.SetTrigger("Shoot");    // Play the single shoot animation
            }
        }
    }

    // Shoot Gun
    public void Shoot()
    {
        readyToShoot = false;   // Can't shoot if already shooting

        // Play the muzzle flash animation
        muzzleFlash.Play();

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate Direction With Spread
        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, direction, out hit))    // Shoot a raycast
        {
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>(); // Grab the enemy health script off the object hit

            if (target != null) // If the raycast hits an enemy
            {
                target.TakeDamage(gunDamage);   // Apply damage to enemy
            }

            // Create the impact particle effect wherever the shot hits, then destroys it after 1 second
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);
        }

        // Load the shoot sound and play the effect
        audioSource.clip = shootSound;
        audioSource.Play();

        bulletsLeft--;                              // Lose ammo for each shot
        bulletsShot--;                              // Count how many bullets have been shot
        Invoke("ResetShot", timeBetweenShooting);   // Allows gun to be shot again after cooldown

        if (bulletsShot > 0 && bulletsLeft > 0) // If the player still has bullets...
        {
            Invoke("Shoot", timeBetweenShots);  // Shoot the gun after timeBetweenShots is up
        }
    }

    // Reset Shooting Control
    private void ResetShot()
    {
        readyToShoot = true;    // Make gun shootable again
    }    

    // Reload Gun
    public void Reload()
    {
        if (bulletsLeft < magSize && !isReloading)
        {
            // Load the reload sound and play the effect
            audioSource.clip = reloadSound;
            audioSource.Play();

            isReloading = true;                     // Set player as reloading
            gunAnimator.SetTrigger("Reload");       // Play the reload animation
        }
    }

    // Reload Completed (called at end of reload animation)
    private void ReloadFinished()
    {
        bulletsLeft = magSize;  // Refill magazine
        isReloading = false;    // Player is finished reloading
    }
}