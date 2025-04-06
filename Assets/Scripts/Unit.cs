using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private float hp = 1000;
    public float HP { get { return hp; } set { hp = value; } }
    [SerializeField] private float atk;
    public float ATK { get { return atk; } set { atk = value; } }
    [SerializeField] private float def;
    public float DEF { get { return def; } set { def = value; } }
    [SerializeField] private float movSPD = 2f;
    public float MovSPD { get { return movSPD; } set { movSPD = value; } }
    [SerializeField] private float atkSPD = 1f;
    public float AtkSPD { get { return atkSPD; } set { atkSPD = value; } }
    [SerializeField] private float range = 1.5f;
    public float Range { get { return range; } set { range = value; } }
    private float currentHP;
    private float ratio = 100;

    private NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } set { agent = value; } }
    private float attackCooldown;
    public float AttackCooldown { get { return attackCooldown; } set { attackCooldown = value; } }
    private NavMeshObstacle obstacle;
    private bool isEnemy;
    public bool IsEnemy { get { return isEnemy; } set { isEnemy = value; } }
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint; // Optional, where the projectile comes from
    [SerializeField] private bool isMelee;
    [SerializeField] private bool isSupport;
    [SerializeField] private Unit currentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movSPD;
        agent.stoppingDistance = range;
        agent.updateRotation = false;

        currentHP = HP;
    }

    bool NavMeshCheck()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is NOT on the NavMesh!");
            return false;
        }
        return true;
    }

    void Update()
    {
        if (!NavMeshCheck())
            return;

        attackCooldown -= Time.deltaTime;

        if (currentTarget == null || currentTarget.HP <= 0)
        {
            if(isSupport == true)
            {
                currentTarget = FindClosestAlly();
            }
            else
            {
                currentTarget = FindClosestEnemy();
            }

            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.transform.position);
                agent.transform.LookAt(currentTarget.transform.position); //look at target
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

                if (attackCooldown <= 0f && isMelee)
                {
                    StartCoroutine(Shake(0.15f, 0.05f));
                    currentTarget.TakeDamage(ATK);
                    attackCooldown = 1f / atkSPD;
                }
                else if (attackCooldown <= 0f && !isMelee)
                {
                    FireProjectile();
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

    void FireProjectile()
    {
        if (projectilePrefab == null || currentTarget == null) return;

        Transform spawnPoint = projectileSpawnPoint != null ? projectileSpawnPoint : transform;

        GameObject proj = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.SetTarget(currentTarget);
        projectile.damage = ATK;
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
        currentHP -= ( amount * ( 1 - def / ratio ) );
        Debug.Log($"took damage");
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateUnitHealthUI(currentHP, HP);
        }
        if (currentHP <= 0)
        {
            Debug.Log("DEADGE");
            Destroy(gameObject);
        }
    }

    Unit FindClosestAlly()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        Unit closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject obj in allUnits)
        {
            Unit unit = obj.GetComponent<Unit>();

            if (unit == null || unit == this || unit.IsEnemy != this.IsEnemy || unit.HP <= 0)
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
}