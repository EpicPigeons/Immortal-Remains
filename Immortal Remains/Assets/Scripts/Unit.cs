using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    //Unit Base Stats
    [SerializeField] private float hp = 1000;
    [SerializeField] private float HP { get { return hp; } }
    [SerializeField] private float atk;
    [SerializeField] private float ATK { get { return atk; } }
    [SerializeField] private float def;
    [SerializeField] private float DEF { get { return def; } }
    [SerializeField] private float movSPD = 2f;
    [SerializeField] private float MovSPD { get { return movSPD; } }
    [SerializeField] private float atkSPD = 1f;
    [SerializeField] private float AtkSPD { get { return atkSPD; } }
    [SerializeField] private float range = 1.5f;
    [SerializeField] private float Range { get { return range; } }
    [SerializeField] private bool isRanged;
    [SerializeField] private float IsRanged { get { return isRanged; } }
    [SerializeField] private string element;
    [SerializeField] private float Element { get { return element; } }
    [SerializeField] private string role;
    [SerializeField] private float Role { get { return role; } }


    private float currenthp;
    private NavMeshAgent agent;
    private bool isEnemy;
    private float attackCooldown;
    private NavMeshObstacle obstacle;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;


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
        agent.stoppingDistance = range;
        agent.updateRotation = false;

        currenthp = hp;
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} not on the NavMesh");
            return;
        }

        attackCooldown -= Time.deltaTime;

        if (currentTarget == null || currentTarget.hp <= 0)
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

            if (distance > range)
            {
                agent.SetDestination(currentTarget.transform.position);
            }
            else
            {
                agent.ResetPath();

                if (attackCooldown <= 0f && !isRanged)
                {
                    StartCoroutine(Shake(0.15f, 0.05f));
                    currentTarget.TakeDamage(atk);
                    attackCooldown = 1f / atkSPD;
                }
                else if (attackCooldown <= 0f)
                {
                    FireProjectile();
                    attackCooldown = 1f / atkSPD;
                }

            }
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null || currentTarget == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 1.2f;

        if (projectileSpawnPoint != null)
        {
            spawnPos = projectileSpawnPoint.position;
        }

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.SetTarget(currentTarget);
        projectile.damage = atk;
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

            if (unit == null || unit == this || unit.IsEnemy == this.IsEnemy || unit.hp <= 0)
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
        currenthp -= amount;
        Debug.Log($"took damage");
        if (currenthp <= 0)
        {
            Debug.Log("DEADGE");
            Destroy(gameObject);
        }
    }
}
