using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup OptionPanel;

    void Start()
    {
        // Ensure the cursor is unlocked and visible when the main menu loads
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Load the game scene
    }

    public void Controls()
    {
        // Show the Controls panel
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

    public void Back()
    {
        // Hide the Controls panel
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }

    // Quit application function
    public void QuitGame()
    {
        Application.Quit();
    }
}