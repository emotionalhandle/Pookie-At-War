using UnityEngine;
using UnityEngine.UI; // Add this for UI components
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Material unclaimedMaterial;
    [SerializeField] private Material claimedMaterial;
    [SerializeField] private GameObject unitPrefab; // Add reference to the unit prefab
    [SerializeField] private float unitSpawnInterval = 5f; // Time between unit spawns
    [SerializeField] private float spawnRadius = 2f; // Distance from spawn point to place new units
    [SerializeField] private float claimRange = 5f; // Range within which units can claim this point
    [SerializeField] private int maxUnitCapacity = 10;
    private Renderer rend;
    private Color originalColor;
    private bool isPlayerUnitInRange = false; // Track if a player unit is in range
    private List<Unit> spawnedUnits = new List<Unit>();
    
    public bool IsClaimed { get; private set; }
    public int OwnerID { get; private set; } = -1;  // -1 = unclaimed, 0+ = general IDs
    public General ControllingGeneral { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = unclaimedMaterial;
        originalColor = rend.material.color;
    }

    public void SetOwnership(int newOwnerID, General general)
    {
        Debug.Log($"Spawn point claimed by general {newOwnerID}");
        IsClaimed = true;
        OwnerID = newOwnerID;
        ControllingGeneral = general;
        
        // Update appearance
        rend.material = claimedMaterial;
        if (general != null)
        {
            rend.material.color = general.GetGeneralColor();
            general.AddSpawnPoint(this);
        }
        
        // Start producing units for the new owner
        StartCoroutine(ProduceUnits());
    }

    private IEnumerator ProduceUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(unitSpawnInterval);
            
            // Clean up any destroyed units from our list
            spawnedUnits.RemoveAll(unit => unit == null);
            
            if (spawnedUnits.Count >= maxUnitCapacity)
            {
                continue; // Skip spawning if at capacity
            }
            
            if (unitPrefab != null && ControllingGeneral != null)
            {
                // Generate a random position around the spawn point
                float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0f, Mathf.Sin(randomAngle));
                Vector3 spawnPosition = transform.position + randomDirection * spawnRadius;
                spawnPosition.y = transform.position.y + 0.5f;
                
                // Instantiate the unit
                GameObject unitObject = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
                
                // Set the unit's owner to the controlling general
                Unit unit = unitObject.GetComponent<Unit>();
                if (unit != null)
                {
                    ControllingGeneral.AddUnit(unit);
                    spawnedUnits.Add(unit);
                    Debug.Log($"Unit spawned at {spawnPosition} for general {OwnerID}. Current count: {spawnedUnits.Count}/{maxUnitCapacity}");
                }
                else
                {
                    Debug.LogWarning("Spawned object does not have a Unit component");
                }
            }
            else if (unitPrefab == null)
            {
                Debug.LogError("No unit prefab assigned to spawn point");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw claim range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, claimRange);
        
        // Draw spawn radius in a different color
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private void Update()
    {
        if (!IsClaimed)
        {
            bool wasInRange = isPlayerUnitInRange;
            isPlayerUnitInRange = false;

            // Check for units in range
            Collider[] colliders = Physics.OverlapSphere(transform.position, claimRange);
            foreach (Collider col in colliders)
            {
                Unit unit = col.GetComponent<Unit>();
                if (unit != null && unit.OwnerID == -1) // Check if it's an unclaimed unit
                {
                    isPlayerUnitInRange = true;
                    if (!wasInRange)
                    {
                        Debug.Log($"Unit in range of spawn point at {transform.position}");
                        SpawnPointManager.Instance.SetSelectedSpawnPoint(this);
                        UIManager.Instance.ShowClaimInterface(this);
                    }
                    break;
                }
            }
            
            // Only hide interface if state changed from in-range to out-of-range
            if (wasInRange && !isPlayerUnitInRange)
            {
                Debug.Log($"No units in range of spawn point at {transform.position}");
                SpawnPointManager.Instance.SetSelectedSpawnPoint(null);
                UIManager.Instance.HideClaimInterface();
            }
        }
    }
}
