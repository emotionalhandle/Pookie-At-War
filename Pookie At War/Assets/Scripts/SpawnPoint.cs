using UnityEngine;
using UnityEngine.UI; // Add this for UI components
using TMPro;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Material unclaimedMaterial;
    [SerializeField] private Material claimedMaterial;
    [SerializeField] private GameObject unitPrefab; // Add reference to the unit prefab
    [SerializeField] private float unitSpawnInterval = 5f; // Time between unit spawns
    [SerializeField] private float spawnRadius = 2f; // Distance from spawn point to place new units
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
            yield return new WaitForSeconds(unitSpawnInterval);
            
            if (unitPrefab != null)
            {
                // Generate a random position around the spawn point
                float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0f, Mathf.Sin(randomAngle));
                Vector3 spawnPosition = transform.position + randomDirection * spawnRadius;
                
                // Ensure the unit is above the ground
                spawnPosition.y = transform.position.y + 0.5f;
                
                // Instantiate the unit
                GameObject unitObject = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
                
                // Set the unit's owner
                Unit unit = unitObject.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.SetOwner(OwnerID);
                    Debug.Log($"Unit spawned at {spawnPosition} for owner {OwnerID}");
                }
                else
                {
                    Debug.LogWarning("Spawned object does not have a Unit component");
                }
            }
            else
            {
                Debug.LogError("No unit prefab assigned to spawn point");
            }
        }
    }
}
