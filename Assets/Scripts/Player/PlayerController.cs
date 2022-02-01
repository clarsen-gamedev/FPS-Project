// Name: PlayerController.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Serialized Variables
    [Header("Camera Controls")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] Camera radarCamara = null;
    [SerializeField] float lookSpeed = 3.5f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [Header("Movement Controls")]
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    [Header("Weapon Controls")]
    [SerializeField] GameObject weaponHandler;
    [SerializeField] GameObject equippedWeapon;

    [Header("Jumping Parameters")]
    [SerializeField] float jumpForce = 8.0f;
    [SerializeField] float gravity = -13.0f;

    [Header("Functional Options")]
    [SerializeField] bool lockCursor = true;

    // Private Variables
    CharacterController controller = null;
    GunSystem gunSystem = null;
    bool isShooting = false;
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    Vector2 targetCameraDelta = Vector2.zero;
    Vector2 currentCameraDelta = Vector2.zero;
    Vector2 currentCameraDeltaVelocity = Vector2.zero;
    Vector2 targetDir = Vector2.zero;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    Quaternion radarRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();       // Grabs the character controller from the player
        gunSystem = equippedWeapon.GetComponent<GunSystem>();   // Grabs the gun system from the equipped weapon
        radarRotation = radarCamara.transform.rotation;         // Store the initial rotation of the radar camera

        if (lockCursor) // Check to see if cursor is locked
        {
            Cursor.lockState = CursorLockMode.Locked;   // Lock the cursor to the center of the screen
            Cursor.visible = false;                     // Hide the cursor
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Stop camera and player movement if game is paused
        if (GameObject.Find("UI").GetComponent<PauseMenu>().GetPauseState())
        {
            return;
        }
        else
        {
            UpdateCamera();     // Move the camera
            UpdateMovement();   // Move the player
        }
    }

    // Player Camera Controls
    public void Camera(InputAction.CallbackContext context)
    {
        targetCameraDelta = context.ReadValue<Vector2>();   // Store input as a Vector2
    }

    // Player Movement Controls
    public void Movement(InputAction.CallbackContext context)
    {
        // Player Movement
        targetDir = context.ReadValue<Vector2>();   // Store input into the currentDir Vector 2
        targetDir.Normalize();                      // Normalize the movement vector

        // Weapon Animations
        if (targetDir.x != 0 || targetDir.y != 0)
        {
            equippedWeapon.GetComponent<Animator>().SetBool("isWalking", true);     // Play the walking animation for the equipped weapon
        }
        else
        {
            equippedWeapon.GetComponent<Animator>().SetBool("isWalking", false);    // Play the idle animation for the equipped weapon
        }
    }

    // Player Jump Controls
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)  // Check to see if the button has been pressed
        {
            if (controller.isGrounded)  // Check to see if player is on the ground
            {
                velocityY = 0.0f;       // Reset the downward velocity variable
                velocityY = jumpForce;  // Player jumps
            }
        }
    }

    // Player Shoot Controls
    public void Shoot(InputAction.CallbackContext context)
    {
        // Full Auto Weapon
        if (gunSystem.allowTriggerHold == true)
        {
            isShooting = context.performed;
        }

        // Semi Auto Weapon
        else if (gunSystem.allowTriggerHold == false)
        {
            isShooting = context.started;
            if (context.performed)
            {
                isShooting = false;
            }
        }
    }

    // Player Reload Controls
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)  // Check to see if the button has been pressed
        {
            gunSystem.Reload(); // Reload the gun
        }
    }

    //// Player Swap Weapon Controls
    //public void SwapWeapon(InputAction.CallbackContext context)
    //{
    //    if (context.performed)  // Check to see if the button has been pressed
    //    {
    //        int selectedWeapon = weaponHandler.GetComponent<WeaponSwap>().selectedWeapon;   // Grab the selectedWeapon variable from weaponHandler

    //        if (selectedWeapon >= weaponHandler.GetComponent<WeaponSwap>().transform.childCount - 1)    // Check to see if the index goes over number of weapons
    //        {
    //            selectedWeapon = 0; // Reset the index
    //        }
    //        else
    //        {
    //            selectedWeapon++;   // Increase the index
    //        }

    //        weaponHandler.GetComponent<WeaponSwap>().selectedWeapon = selectedWeapon;   // Return the index

    //        equippedWeapon = weaponHandler.GetComponent<WeaponSwap>().SelectWeapon();   // Swap the player's weapon
    //    }
    //}

    // Update the camera movement
    private void UpdateCamera()
    {
        currentCameraDelta = Vector2.SmoothDamp(currentCameraDelta, targetCameraDelta, ref currentCameraDeltaVelocity, mouseSmoothTime);    // Smooth camera movement

        cameraPitch -= currentCameraDelta.y * lookSpeed;                // Pitch camera on the Y axis
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);          // Stop camera from rotating past -90 and 90 degrees
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;    // Rotate camera around right pitch
        transform.Rotate(Vector3.up, currentCameraDelta.x * lookSpeed); // Rotate the player on the X axis

        radarCamara.transform.rotation = radarRotation;                 // Reset rotation of the radar
    }

    // Update the player movement
    private void UpdateMovement()
    {
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);                                 // Smooth the player movement
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;    // Store movement as velocity vector
        controller.Move(velocity * Time.deltaTime);                                                                                     // Move the player using the controller

        // Update gravity
        velocityY += gravity * Time.deltaTime;
    }

    // Used to send the Shoot input to the Gun System script
    public bool GetIsShooting()
    {
        return isShooting;
    }
}