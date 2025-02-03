using UnityEngine;

public class Unit :MonoBehaviour
{
    private float unitHealth;
    public float unitMaxHealth = 100f;

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
    

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnits.Remove(gameObject);
    }
}
