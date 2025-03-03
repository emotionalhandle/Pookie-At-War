using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    
    private void Start()
    {
        // Add listeners to buttons
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
        }
    }
    
    public void OnResumeClicked()
    {
        // Call the UIManager's ResumeGame method
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResumeGame();
        }
    }
    
    public void OnQuitClicked()
    {
        Debug.Log("Quit button clicked - returning to main menu or quitting game");
        
        // If there's a main menu scene, load it
        // SceneManager.LoadScene("MainMenu");
        
        // Or quit the application if in a standalone build
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
        
        // If in the Unity Editor, this will just log a message
        #if UNITY_EDITOR
        Debug.Log("Application.Quit() called in Editor - would quit in build");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
} 