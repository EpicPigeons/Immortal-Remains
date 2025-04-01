using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private bool hasUnit = false;
    public bool HasUnit
    {
        get
        {
            return hasUnit;
        }
        set
        {
            hasUnit = value;
        }
    }
    private GameObject currentUnit;
    private int rowIndex;
    public int RowIndex
    {
        get
        {
            return rowIndex;
        }
        set
        {
            rowIndex = value;
        }
    }
    private bool isEnemyCell = false;
    public bool IsEnemyCell
    {
        get
        {
            return isEnemyCell;
        }
        set
        {
            isEnemyCell = value;
        }
    }

    public void SpawnUnit(GameObject prefab, bool isEnemy)
    {
        if (hasUnit) return;

        Bounds bounds = GetComponent<Collider>().bounds;
        Vector3 spawnPosition = new Vector3(
            bounds.center.x,
            bounds.max.y,
            bounds.center.z
        );

        currentUnit = Instantiate(prefab, spawnPosition, Quaternion.identity);
        currentUnit.GetComponent<Unit>().IsEnemy = isEnemy;
        hasUnit = true;
    }

    public void RemoveUnit()
    {
        if (!HasUnit || currentUnit == null) return;

        Destroy(currentUnit);
        HasUnit = false;
        currentUnit = null;
    }
}
