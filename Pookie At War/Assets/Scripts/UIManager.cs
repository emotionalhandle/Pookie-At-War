using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Button claimButton;
    
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
        SpawnPointManager.Instance.ClaimSpawnPoint(0);  // 0 = player ID
        HideClaimInterface();
    }
}
