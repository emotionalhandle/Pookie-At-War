using UnityEngine;
using UnityEngine.UI; // Add this for UI components
using TMPro;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Material unclaimedMaterial;
    [SerializeField] private Material claimedMaterial;
    private Renderer rend;
    private Color originalColor;
    
    public bool IsClaimed { get; private set; }
    public int OwnerID { get; private set; } = -1;  // -1 = unclaimed, 0 = player, 1+ = AI clans

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = unclaimedMaterial;
        originalColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        if (!IsClaimed)
        {
            Debug.Log($"Spawn point selected at {transform.position}");
            
            // Highlight this spawn point
            rend.material.color = new Color(
                originalColor.r + 0.2f,
                originalColor.g + 0.2f,
                originalColor.b + 0.2f,
                originalColor.a
            );
            
            // Notify the UI manager to show claim interface
            UIManager.Instance.ShowClaimInterface(this);
            
            // Register this as the selected spawn point
            SpawnPointManager.Instance.SetSelectedSpawnPoint(this);
        }
    }

    public void SetOwnership(int newOwnerID)
    {
        Debug.Log($"Spawn point claimed by owner {newOwnerID}");
        IsClaimed = true;
        OwnerID = newOwnerID;
        rend.material = claimedMaterial;
        
        // Start producing units for the new owner
        StartCoroutine(ProduceUnits());
    }

    private IEnumerator ProduceUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log($"Unit created at spawn point {transform.position}");
        }
    }
}
