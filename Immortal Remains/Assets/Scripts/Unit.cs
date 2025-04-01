using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float HP = 1000;
    [SerializeField] private float ATK;
    [SerializeField] private float DEF;
    [SerializeField] private float movSPD = 2f;
    [SerializeField] private float atkSPD = 1f;
    private bool isEnemy;
    public bool IsEnemy
    {
        get
        {
            return isEnemy;
        }
        set
        {
            isEnemy = value;
        }
    }
    [SerializeField] private float detectionRange;

    [SerializeField] private Unit currentTarget;

    void Awake()
    {
    }
    void Update()
    {
        if (currentTarget == null || currentTarget.HP <= 0)
        {
            currentTarget = FindClosestEnemy();
            Debug.Log("target found");
        }

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > detectionRange)
            {
                Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                transform.position += direction * movSPD * Time.deltaTime;
                Debug.Log("moving");
            }
            else
            {

            }
        }
    }

    Unit FindClosestEnemy()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        Unit closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject obj in allUnits)
        {
            Unit unit = obj.GetComponent<Unit>();
            Debug.Log("" + unit.IsEnemy + "" + this.IsEnemy);
            if (unit.IsEnemy == this.IsEnemy || unit.HP <= 0)
                continue;

            Debug.Log("inside foreach");
            float dist = Vector3.Distance(transform.position, unit.transform.position);
            Debug.Log(dist);


            closestDist = dist;
            closest = unit;
        }

        Debug.Log(closest);

        return closest;
    }

    public void TakeDamage(float amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
