using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel; // Assign the tutorial panel in the Inspector
    [SerializeField] private GameObject creditsPanel;
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
        LevelManager.Instance.LoadScene("MainGameplay");
    }

    // Toggle background music on or off
    public void ToggleAudio()
    {
        isMusicOn = !isMusicOn;

        if (backgroundMusic != null)
        {
            backgroundMusic.mute = !isMusicOn;
        }

        
    }

    // Show the tutorial panel
    public void ShowTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        
    }

    // Hide the tutorial panel
    public void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
    }

    public void ShowCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    // Exit the game
    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
}
