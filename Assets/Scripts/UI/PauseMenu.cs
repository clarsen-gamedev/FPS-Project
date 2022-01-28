using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Public Variables
    public static bool GamePaused = false;
    public GameObject gameUI;
    public GameObject pauseMenu;

    // Private Variables
    [SerializeField] KeyCode pauseButton = KeyCode.Escape;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(pauseButton))  // If the player pushes the pause button
        {
            if (GamePaused) // If game is currently paused
            {
                ResumeGame();   // Resume gameplay
            }
            else
            {
                PauseGame();    // Pause the game
            }    
        }
    }

    // Pause Game Function
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;                  // Show the cursor

        pauseMenu.SetActive(true);  // Pause menu becomes visible
        gameUI.SetActive(false);    // Hide game UI
        Time.timeScale = 0f;        // Stops the time scale, pausing gameplay
        GamePaused = true;          // Set GamePaused to true
    }

    // Resume Game Function
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;   // Lock the cursor to the center of the screen
        Cursor.visible = false;                     // Hide the cursor

        pauseMenu.SetActive(false); // Hides the pause menu
        gameUI.SetActive(true);     // Game UI becomes visible
        Time.timeScale = 1f;        // Resets the time scale, resuming gameplay
        GamePaused = false;         // Set GamePaused to false
    }

    // Quit Game Function
    public void QuitGame()
    {
        Application.Quit();
    }

    // Grab Pause Status
    public bool GetPauseState()
    {
        return GamePaused;
    }
}