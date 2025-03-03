using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set;}

    public List<GameObject> unitsSelected = new List<GameObject>();
    public List<GameObject> allUnits = new List<GameObject>();
    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackable;
    public bool attackCursorVisible;
    public GameObject groundMarker;
    public Camera cam;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    public void DragSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }

    private void SelectByClick(GameObject unit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            DeselectAll();
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void EnableUnitMovement(GameObject unit, bool enable)
    {
        unit.GetComponent<UnitMovement>().enabled = enable;
    }

    public void DeselectAll()
    {
        foreach (GameObject unit in unitsSelected)
        {
            SelectUnit(unit, false);
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    private void MultiSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Kill selected units when K is pressed
        if (Input.GetKeyDown(KeyCode.K) && unitsSelected.Count > 0)
        {
            // Create a new list to avoid modification during enumeration
            List<GameObject> unitsToKill = new List<GameObject>(unitsSelected);
            foreach (GameObject unit in unitsToKill)
            {
                KillUnit(unit);
            }
            // Clear selection after killing units
            unitsSelected.Clear();
            groundMarker.SetActive(false);
            return; // Skip the rest of the update for this frame
        }

        // Only process selection and movement if we have valid units
        List<GameObject> validUnits = unitsSelected.FindAll(unit => unit != null);
        if (validUnits.Count != unitsSelected.Count)
        {
            // Clean up any null references
            unitsSelected = validUnits;
            if (unitsSelected.Count == 0)
            {
                groundMarker.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else if (!Input.GetKey(KeyCode.LeftShift))
                {
                    SelectByClick(hit.collider.gameObject);
                }
            }
            else
            {
                DeselectAll();
            }
        }

            // ground marker
        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ground");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(true);
            }
        }

        // Attack Target //

        if (unitsSelected.Count > 0 && AtLeastOneOffensiveUnit(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy hovered with mouse");

                attackCursorVisible = true;

                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;   

                    foreach (GameObject unit in unitsSelected)
                    {
                        if (unit.GetComponent<AttackController>() != null)
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            }
            else
            {
                attackCursorVisible = false;
            }
        }

    }

    private bool AtLeastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }


    private void TriggerSelectionIndicator(GameObject unit, bool isSelected)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isSelected);
    }

    public void KillUnit(GameObject unit)
    {
        Unit unitComponent = unit.GetComponent<Unit>();
        if (unitComponent != null)
        {
            // Deal enough damage to kill the unit
            // The TakeDamage function will handle cleanup and destruction
            unitComponent.TakeDamage((int)unitComponent.unitMaxHealth);
        }
        else
        {
            // Fallback to old behavior if somehow the Unit component is missing
            if (unitsSelected.Contains(unit))
            {
                SelectUnit(unit, false);
                unitsSelected.Remove(unit);
            }
            
            if (allUnits.Contains(unit))
            {
                allUnits.Remove(unit);
            }

            // If any selected units were targeting this unit for attack, clear their target
            foreach (GameObject selectedUnit in unitsSelected)
            {
                AttackController attackController = selectedUnit.GetComponent<AttackController>();
                if (attackController != null && attackController.targetToAttack == unit.transform)
                {
                    attackController.targetToAttack = null;
                }
            }

            Destroy(unit);
        }
    }
}
