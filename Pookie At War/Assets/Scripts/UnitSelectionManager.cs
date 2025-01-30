using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set;}

    public List<GameObject> unitsSelected = new List<GameObject>();
    public List<GameObject> allUnits = new List<GameObject>();
    public LayerMask clickable;
    public LayerMask ground;
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
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isSelected)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isSelected);
    }
}
