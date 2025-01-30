using UnityEngine;

public class Unit :MonoBehaviour
{
    void Start()
    {
        UnitSelectionManager.Instance.allUnits.Add(gameObject);
    }
    
    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnits.Remove(gameObject);
    }
}
