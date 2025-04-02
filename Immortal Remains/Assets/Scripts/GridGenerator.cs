using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField][Range(6,12)] private int columns = 6;
    [SerializeField][Range(6,12)] private int rows = 6;
    [SerializeField] private float cellSize = 1f;
    private int allyRows;
    private int enemyRows;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Material whiteMat;
    [SerializeField] private Material blackMat;
    private List<Cell> allCells;
    private GameManager gm;
    [SerializeField] private NavMeshSurface navSurface;

    void Awake()
    {
        allyRows = rows/2;
        enemyRows = rows/2;
        allCells = new List<Cell>();
    }
    void Start()
    {
        GenerateGrid();
        navSurface.BuildNavMesh();
        gm = FindObjectOfType<GameManager>();
        gm.AllCells = allCells.ToArray();
        gm.EnemySpawnManager();
    }

    private void GenerateGrid()
    {
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 offset = new Vector3(columns * cellSize, 0, rows * cellSize) * 0.5f;
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize) - offset;

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{z}";

                Renderer renderer = cell.GetComponent<Renderer>();
                if ((x + z) % 2 == 0)
                    renderer.material = whiteMat;
                else
                    renderer.material = blackMat;

                Cell cellScript = cell.GetComponent<Cell>();
                if (cellScript != null)
                {
                    cellScript.RowIndex = z;
                }

                cell.tag = "Cell";

                if (x < allyRows)
                    cellScript.IsEnemyCell = false;
                if (x >= rows - enemyRows)
                    cellScript.IsEnemyCell = true;

                allCells.Add(cellScript);
            }
        }
    }

}
