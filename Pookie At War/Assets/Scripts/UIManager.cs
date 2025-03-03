using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject pausePanel; // Reference to the pause panel
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        claimButton.interactable = false;  // Start with button disabled
        
        // Make sure pause panel is hidden at start
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void ShowClaimInterface(SpawnPoint spawnPoint)
    {
        Debug.Log("Enabling claim button");
        claimButton.interactable = true;
    }

    public void HideClaimInterface()
    {
        Debug.Log("Disabling claim button");
        claimButton.interactable = false;
    }

    public void OnClaimButtonClicked()
    {
        Debug.Log("Claim button clicked");
        
        // Get all player-controlled generals
        var playerGenerals = GeneralManager.Instance.GetPlayerControlledGenerals();
        
        if (playerGenerals.Count > 0)
        {
            // For now, use the first player general. You could add UI to select which general to use
            var general = playerGenerals[0];
            SpawnPointManager.Instance.ClaimSpawnPoint(general.GeneralID, general);
        }
        else
        {
            Debug.LogError("No player-controlled generals found!");
        }
        HideClaimInterface();
    }
    
    // Toggle the pause panel visibility
    public void TogglePausePanel()
    {
        if (pausePanel != null)
        {
            bool isActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isActive);
            Debug.Log($"Pause panel is now {(isActive ? "hidden" : "visible")}");
        }
        else
        {
            Debug.LogWarning("Pause panel reference is missing in UIManager");
        }
    }
    
    // Method to resume the game (hide pause panel and enable camera movement)
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        if (RTSCameraController.instance != null)
        {
            RTSCameraController.instance.cameraMovementEnabled = true;
        }
    }
}
