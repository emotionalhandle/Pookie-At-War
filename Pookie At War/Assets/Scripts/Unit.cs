using UnityEngine;

public class Unit :MonoBehaviour
{
    private float unitHealth;
    public float unitMaxHealth = 100f;
    public int OwnerID { get; private set; } = -1; // -1 = unclaimed, 0 = player, 1+ = AI clans

    public HealthTracker healthTracker;
    
    void Start()
    {
        UnitSelectionManager.Instance.allUnits.Add(gameObject);

        unitHealth = unitMaxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        if (unitHealth <= 0)
        {
            // Dying logic
            Destroy(gameObject);
        }
    }

    internal void TakeDamage(int damage)
    {
        unitHealth -= damage;
        UpdateHealthUI();
    }
    
    public void SetOwner(int newOwnerID)
    {
        OwnerID = newOwnerID;
        // You could add additional logic here, like changing the unit's color based on owner
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnits.Remove(gameObject);
    }
}
