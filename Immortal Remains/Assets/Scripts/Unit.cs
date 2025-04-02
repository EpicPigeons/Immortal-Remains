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
    private float currentHP;

    private NavMeshAgent agent;
    private bool isEnemy;
    private float attackCooldown;
    private NavMeshObstacle obstacle;


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
        agent.updateRotation = false;
        
        currentHP = HP;
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is NOT on the NavMesh!");
            return;
        }

        attackCooldown -= Time.deltaTime;

        if (currentTarget == null || currentTarget.HP <= 0)
        {
            currentTarget = FindClosestEnemy();

            if (currentTarget != null)
            {
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

                if (attackCooldown <= 0f)
                {
                    StartCoroutine(Shake(0.15f, 0.05f));
                    currentTarget.TakeDamage(ATK);
                    attackCooldown = 1f / atkSPD;
                }
            }
        }
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetZ = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, 0, offsetZ);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
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
        currentHP -= amount;
        Debug.Log($"took damage");
        if (currentHP <= 0)
        {
            Debug.Log("DEADGE");
            Destroy(gameObject);
        }
    }
}
