using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rangedUnit;
    public GameObject RangedUnit
    {
        get
        {
            return rangedUnit;
        }
    }
    [SerializeField] private GameObject meleeUnit;
    public GameObject MeleeUnit
    {
        get
        {
            return meleeUnit;
        }
    }
    [SerializeField] private LayerMask cellLayer;
    public LayerMask CellLayer
    {
        get
        {
            return cellLayer;
        }
    }
    [SerializeField] private Material allyMaterial;
    public Material AllyMaterial
    {
        get
        {
            return allyMaterial;
        }

    }
    [SerializeField] private Material enemyMaterial;
    public Material EnemyMaterial
    {
        get
        {
            return enemyMaterial;
        }
    }
    private GameObject currentUnitPrefab;
    public GameObject CurrentUnitPrefab
    {
        get
        {
            return currentUnitPrefab;
        }
        set
        {
            currentUnitPrefab = value;
        }
    }
    [SerializeField][Range(2, 12)] private int enemyCount = 5;
    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        set
        {
            enemyCount = value;
        }
    }
    [SerializeField] private int partySize;
    public int PartySize { get { return partySize; } set { partySize = value; } }
    private int unitCount;
    public int UnitCount
    {
        get
        {
            return unitCount;
        }
        set
        {
            unitCount = value;
        }
    }
    private Cell[] allCells;
    [SerializeField] private SpawnManager spawnManager;
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
    // TODO: Reduce the getter and setter calls later after confirming which ones are absolutely necessary and which ones can be sent through argument
    void Awake()
    {
        currentUnitPrefab = rangedUnit;
        unitCount = 0;
    }
    void Start()
    {
    }

    public void EnemySpawnManager()
    {
        spawnManager.EnemySpawn(this);
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
                spawnManager.Spawn(this, true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            spawnManager.Spawn(this, false);
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.mouseScrollDelta.y != 0)
        {
            ToggleUnitType();
        }
    }
}
