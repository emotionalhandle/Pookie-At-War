using UnityEngine;

public class GeneralSpawner : MonoBehaviour
{
    [SerializeField] private GameObject generalPrefab; // Drag your general prefab here
    
    public General SpawnGeneral(Vector3 position, string name, Color color, bool isPlayerControlled)
    {
        GameObject generalObj = Instantiate(generalPrefab, position, Quaternion.identity);
        General general = generalObj.GetComponent<General>();
        
        if (general != null)
        {
            general.SetPlayerControlled(isPlayerControlled);
            // Name the GameObject for easy identification
            generalObj.name = $"General_{name}";
        }
        
        return general;
    }

    // Example usage in Start() - you can modify or remove this
    private void Start()
    {
        // Spawn a player general
        SpawnGeneral(new Vector3(0, 0, 0), "Player", Color.blue, true);
        
        // Spawn some AI generals
        SpawnGeneral(new Vector3(10, 0, 10), "AI_1", Color.red, false);
        SpawnGeneral(new Vector3(-10, 0, -10), "AI_2", Color.green, false);
    }
} 