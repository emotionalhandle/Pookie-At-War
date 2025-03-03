using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private float followDistance = 5f; // How close to get to the general
    [SerializeField] private float updatePathInterval = 0.5f; // How often to update the path
    private NavMeshAgent agent;
    private float nextPathUpdate;

    private float unitHealth;
    public float unitMaxHealth = 100f;
    public int OwnerID { get; private set; } = -1; // -1 = unclaimed, 0+ = general IDs
    public General ControllingGeneral { get; private set; }

    public HealthTracker healthTracker;
    private Renderer rend;
    private Camera mainCamera;
    
    void Start()
    {
        UnitSelectionManager.Instance.allUnits.Add(gameObject);
        rend = GetComponent<Renderer>();
        unitHealth = unitMaxHealth;
        UpdateHealthUI();
        
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent missing on unit {gameObject.name}!");
        }
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in scene!");
        }
    }

    void Update()
    {
        // Make health bar face camera
        if (healthTracker != null && mainCamera != null)
        {
            healthTracker.transform.rotation = mainCamera.transform.rotation;
        }

        if (ControllingGeneral != null && agent != null)
        {
            // Only update path periodically to save performance
            if (Time.time >= nextPathUpdate)
            {
                Vector3 generalPosition = ControllingGeneral.transform.position;
                float distanceToGeneral = Vector3.Distance(transform.position, generalPosition);
                
                // Only move if we're too far from the general
                if (distanceToGeneral > followDistance)
                {
                    agent.SetDestination(generalPosition);
                }
                else
                {
                    agent.ResetPath();
                }
                
                nextPathUpdate = Time.time + updatePathInterval;
            }
        }
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        if (unitHealth <= 0)
        {
            // Dying logic
            if (ControllingGeneral != null)
            {
                ControllingGeneral.RemoveUnit(this);
            }
            Destroy(gameObject);
        }
    }

    internal void TakeDamage(int damage)
    {
        unitHealth -= damage;
        UpdateHealthUI();
    }
    
    public void SetOwner(int newOwnerID, General general = null)
    {
        if (ControllingGeneral != null)
        {
            ControllingGeneral.RemoveUnit(this);
        }
        
        OwnerID = newOwnerID;
        ControllingGeneral = general;
        
        // Update unit appearance based on general's color
        if (rend != null && general != null)
        {
            rend.material.color = general.GetGeneralColor();
        }
    }

    private void OnDestroy()
    {
        if (ControllingGeneral != null)
        {
            ControllingGeneral.RemoveUnit(this);
        }
        UnitSelectionManager.Instance.allUnits.Remove(gameObject);
    }
}
