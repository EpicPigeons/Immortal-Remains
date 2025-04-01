using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private float HP = 1000;
    [SerializeField] private float ATK;
    [SerializeField] private float DEF;
    [SerializeField] private float movSPD = 2f;
    [SerializeField] private float atkSPD = 1f;
    [SerializeField] private float detectionRange = 1.5f;

    private NavMeshAgent agent;
    private bool isEnemy;

    public bool IsEnemy
    {
        get { return isEnemy; }
        set { isEnemy = value; }
    }

    [SerializeField] private Unit currentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movSPD;
        agent.stoppingDistance = detectionRange;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is NOT on the NavMesh!");
            return;
        }

        if (currentTarget == null || currentTarget.HP <= 0)
        {
            currentTarget = FindClosestEnemy();

            if (currentTarget != null)
            {
                Debug.Log($"{gameObject.name} found target: {currentTarget.gameObject.name}");
                agent.SetDestination(currentTarget.transform.position);
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > detectionRange)
            {
                agent.SetDestination(currentTarget.transform.position);
            }
            else
            {
                agent.ResetPath();
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

            if (unit == null || unit == this || unit.IsEnemy == this.IsEnemy || unit.HP <= 0)
                continue;

            float dist = Vector3.Distance(transform.position, unit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = unit;
            }
        }

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
