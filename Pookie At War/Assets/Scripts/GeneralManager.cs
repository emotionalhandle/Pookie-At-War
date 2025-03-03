using UnityEngine;
using System.Collections.Generic;

public class GeneralManager : MonoBehaviour
{
    public static GeneralManager Instance { get; private set; }
    
    private List<General> allGenerals = new List<General>();
    private Dictionary<int, General> generalsById = new Dictionary<int, General>();
    private int nextAvailableID = 1; // Start from 1, reserve 0 for unassigned

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

    public int GetNextAvailableID()
    {
        while (generalsById.ContainsKey(nextAvailableID))
        {
            nextAvailableID++;
        }
        return nextAvailableID++;
    }

    public void RegisterGeneral(General general)
    {
        if (!allGenerals.Contains(general))
        {
            allGenerals.Add(general);
            generalsById[general.GeneralID] = general;
            Debug.Log($"Registered general with ID {general.GeneralID}, Name: {general.GetGeneralName()}, IsPlayer: {general.IsPlayerControlled()}");
        }
    }

    public void UnregisterGeneral(General general)
    {
        allGenerals.Remove(general);
        generalsById.Remove(general.GeneralID);
        Debug.Log($"Unregistered general with ID {general.GeneralID}");
    }

    public General GetGeneralById(int id)
    {
        return generalsById.TryGetValue(id, out General general) ? general : null;
    }

    public List<General> GetAllGenerals()
    {
        return allGenerals;
    }

    public List<General> GetPlayerControlledGenerals()
    {
        var playerGenerals = allGenerals.FindAll(g => g.IsPlayerControlled());
        Debug.Log($"Found {playerGenerals.Count} player-controlled generals");
        return playerGenerals;
    }

    public General GetNearestGeneral(Vector3 position)
    {
        float nearestDistance = float.MaxValue;
        General nearestGeneral = null;

        foreach (General general in allGenerals)
        {
            float distance = Vector3.Distance(position, general.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestGeneral = general;
            }
        }

        return nearestGeneral;
    }

    // Helper method to list all generals in the console
    [ContextMenu("List All Generals")]
    private void ListAllGenerals()
    {
        Debug.Log($"=== All Generals ({allGenerals.Count}) ===");
        foreach (var general in allGenerals)
        {
            Debug.Log($"ID: {general.GeneralID}, Name: {general.GetGeneralName()}, IsPlayer: {general.IsPlayerControlled()}");
        }
    }
} 