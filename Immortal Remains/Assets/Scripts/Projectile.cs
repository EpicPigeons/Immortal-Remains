using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private Unit target;

    public void SetTarget(Unit targetUnit)
    {
        target = targetUnit;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move toward the target
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // Optional: rotate to face target
        transform.LookAt(target.transform);

        // Hit check
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 0.5f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
