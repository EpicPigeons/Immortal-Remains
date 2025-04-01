using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rangedUnit;
    [SerializeField] private GameObject meleeUnit;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private Material allyMaterial;
    [SerializeField] private Material enemyMaterial;
    private GameObject currentUnitPrefab;
    [SerializeField][Range(2, 12)] private int enemyCount = 5;
    [SerializeField] private int partySize;
    private int unitCount;
    private Cell[] allCells;
    public Cell[] AllCells
    {
        get
        {
            return allCells;
        }
        set
        {
            allCells = value;
        }
    }
    private List<Cell> enemyCells;
    // Start is called before the first frame update

    void Awake()
    {
        currentUnitPrefab = rangedUnit;
        unitCount = 0;
    }
    void Start()
    {
    }

    void setEnemyMaterial(GameObject target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = enemyMaterial;
        }
    }

    void CellClickManager(bool spawn)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, cellLayer))
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell == null) return;
            if (cell.IsEnemyCell)
            {
                Debug.Log("Cannot place units in enemy territory!");
                return;
            }

            if (spawn && !cell.HasUnit)
            {
                currentUnitPrefab.GetComponent<Renderer>().material = allyMaterial;
                cell.SpawnUnit(currentUnitPrefab, false);
                unitCount++;
            }
            else if (!spawn && cell.HasUnit)
            {
                cell.RemoveUnit();
                unitCount--;
            }
        }
    }

    public void EnemySpawnManager()
    {
        enemyCells = new List<Cell>();
        foreach (Cell cell in allCells)
        {
            if (cell.IsEnemyCell && !cell.HasUnit)
            {
                Debug.Log("adding cell to enemy list");
                enemyCells.Add(cell);
            }
        }

        if (enemyCells.Count == 0)
        {
            Debug.LogWarning("No valid enemy cells found!");
            return;
        }

        for (int i = 0; i < enemyCells.Count; i++)
        {
            int rand = Random.Range(i, enemyCells.Count);
            (enemyCells[i], enemyCells[rand]) = (enemyCells[rand], enemyCells[i]);
        }

        int spawnCount = Mathf.Min(enemyCount, enemyCells.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject spawnedUnit = Random.value < 0.5f ? meleeUnit : rangedUnit;

            spawnedUnit.GetComponent<Renderer>().material = enemyMaterial;

            enemyCells[i].SpawnUnit(spawnedUnit, true);
        }

        Debug.Log($"Spawned {spawnCount} enemy units.");
    }

    void ToggleUnitType()
    {
        if (currentUnitPrefab == rangedUnit)
        {
            currentUnitPrefab = meleeUnit;
            Debug.Log("Changed to Melee Unit");
        }
        else
        {
            currentUnitPrefab = rangedUnit;
            Debug.Log("Changed to Ranged Unit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (unitCount >= partySize)
                Debug.Log("Party limit reached.");
            else
                CellClickManager(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CellClickManager(false);
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.mouseScrollDelta.y != 0)
        {
            ToggleUnitType();
        }
    }
}
