using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class General : MonoBehaviour
{
    [SerializeField] private string generalName;
    [SerializeField] private Color generalColor = Color.blue;
    [SerializeField] private bool isPlayerControlled = false;
    [SerializeField] private int generalID = 0; // Make this assignable in inspector
    
    public int GeneralID => generalID; // Read-only property that returns the serialized field
    private List<Unit> controlledUnits = new List<Unit>();
    private List<SpawnPoint> controlledSpawnPoints = new List<SpawnPoint>();

    private void Start()
    {
        if (GeneralManager.Instance != null)
        {
            // Let GeneralManager assign an ID if none is set
            if (generalID == 0)
            {
                generalID = GeneralManager.Instance.GetNextAvailableID();
            }
            GeneralManager.Instance.RegisterGeneral(this);
            Debug.Log($"General {generalName} registered with ID: {generalID}");
        }
        else
        {
            Debug.LogError("No GeneralManager found in scene!");
        }
    }

    public bool IsPlayerControlled()
    {
        return isPlayerControlled;
    }

    public void AddUnit(Unit unit)
    {
        if (!controlledUnits.Contains(unit))
        {
            controlledUnits.Add(unit);
            unit.SetOwner(GeneralID, this);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        controlledUnits.Remove(unit);
    }

    public void AddSpawnPoint(SpawnPoint spawnPoint)
    {
        if (!controlledSpawnPoints.Contains(spawnPoint))
        {
            controlledSpawnPoints.Add(spawnPoint);
        }
    }

    public void RemoveSpawnPoint(SpawnPoint spawnPoint)
    {
        controlledSpawnPoints.Remove(spawnPoint);
    }

    public Color GetGeneralColor()
    {
        return generalColor;
    }

    public string GetGeneralName()
    {
        return generalName;
    }

    public List<Unit> GetControlledUnits()
    {
        return controlledUnits;
    }

    public List<SpawnPoint> GetControlledSpawnPoints()
    {
        return controlledSpawnPoints;
    }

    public void SetPlayerControlled(bool isPlayer)
    {
        isPlayerControlled = isPlayer;
    }

    private void OnDestroy()
    {
        if (GeneralManager.Instance != null)
        {
            GeneralManager.Instance.UnregisterGeneral(this);
        }
    }
} 