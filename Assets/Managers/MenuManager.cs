using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel; // Assign the tutorial panel in the Inspector
    [SerializeField] private AudioSource backgroundMusic; // Assign the background music AudioSource

    private bool isMusicOn = true; // Tracks the music state

    private void Start()
    {
        // Ensure the tutorial panel is hidden at the start
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void PlayGame()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene("MainGameplay"); // Replace "GameScene" with the name of your game scene
    }

    // Toggle background music on or off
    public void ToggleAudio()
    {
        isMusicOn = !isMusicOn;

        if (backgroundMusic != null)
        {
            backgroundMusic.mute = !isMusicOn;
        }

        Debug.Log($"Music is now {(isMusicOn ? "On" : "Off")}");
    }

    // Show the tutorial panel
    public void ShowTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        Debug.Log("Tutorial panel opened.");
    }

    // Hide the tutorial panel
    public void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        Debug.Log("Tutorial panel closed.");
    }

    // Exit the game
    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
}
