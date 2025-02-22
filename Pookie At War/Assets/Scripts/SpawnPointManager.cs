using UnityEngine;
using System.Collections.Generic;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance { get; private set; }

    private List<SpawnPoint> allSpawnPoints = new List<SpawnPoint>();
    private SpawnPoint selectedSpawnPoint;
    
    // Dictionary to track spawn points by owner
    private Dictionary<int, List<SpawnPoint>> spawnPointsByOwner = new Dictionary<int, List<SpawnPoint>>();

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

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (!allSpawnPoints.Contains(spawnPoint))
        {
            allSpawnPoints.Add(spawnPoint);
        }
    }

    public void SetSelectedSpawnPoint(SpawnPoint spawnPoint)
    {
        // Deselect previous spawn point if exists
        if (selectedSpawnPoint != null)
        {
            // Reset its appearance
            // You might want to add a ResetAppearance method to SpawnPoint
        }

        selectedSpawnPoint = spawnPoint;
    }

    public void ClaimSpawnPoint(int ownerID)
    {
        if (selectedSpawnPoint != null && !selectedSpawnPoint.IsClaimed)
        {
            selectedSpawnPoint.SetOwnership(ownerID);
            
            // Add to owner's list of spawn points
            if (!spawnPointsByOwner.ContainsKey(ownerID))
            {
                spawnPointsByOwner[ownerID] = new List<SpawnPoint>();
            }
            spawnPointsByOwner[ownerID].Add(selectedSpawnPoint);
            
            selectedSpawnPoint = null;  // Clear selection after claiming
        }
    }

    public List<SpawnPoint> GetSpawnPointsForOwner(int ownerID)
    {
        if (spawnPointsByOwner.ContainsKey(ownerID))
        {
            return spawnPointsByOwner[ownerID];
        }
        return new List<SpawnPoint>();
    }
} 