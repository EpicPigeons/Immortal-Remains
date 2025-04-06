using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasGenerator : MonoBehaviour
{
    [SerializeField] private GameObject unitUIPrefab;
    [SerializeField] private Transform canvasParent;

    public static CanvasGenerator Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CreateUnitUI(Unit unit)
    {
        if (unitUIPrefab == null || canvasParent == null)
        {
            Debug.LogWarning("Unit UI preafab or Canvas Parent is not existing");
            return;
        }

        GameObject uiObject = Instantiate(unitUIPrefab, canvasParent);
        UnitUI unitUI = uiObject.GetComponent<UnitUI>();
        if (unitUI != null)
        {
            unitUI.unit = unit;
        }
        else
        {
            Debug.LogError("Generated UI prefab does not contain UnitUI component");
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Unit clickedUnit = hit.collider.GetComponent<Unit>();
                if (clickedUnit != null && !clickedUnit.IsEnemy)
                {
                    CreateUnitUI(clickedUnit);
                }
            }
        }
    }
}
