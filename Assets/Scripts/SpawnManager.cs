using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public void Spawn(GameManager gm, bool spawn)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, gm.CellLayer))
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
                gm.CurrentUnitPrefab.GetComponent<Renderer>().material = gm.AllyMaterial;
                cell.SpawnUnit(gm.CurrentUnitPrefab, false);
                gm.UnitCount++;
            }
            else if (!spawn && cell.HasUnit)
            {
                cell.RemoveUnit();
                gm.UnitCount--;
            }
        }
    }

    public void EnemySpawn(GameManager gm)
    {
        List<Cell> enemyCells = new List<Cell>();
        foreach (Cell cell in gm.AllCells)
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

        int spawnCount = Mathf.Min(gm.EnemyCount, enemyCells.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject spawnedUnit = gm.EnemyUnitTypes[Mathf.RoundToInt(Random.Range(0, gm.UnitTypes.Length))];
            
            spawnedUnit.GetComponent<Renderer>().material = gm.EnemyMaterial;

            enemyCells[i].SpawnUnit(spawnedUnit, true);
        }

        Debug.Log($"Spawned {spawnCount} enemy units.");
    }
}
