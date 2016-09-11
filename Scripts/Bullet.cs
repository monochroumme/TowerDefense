using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform target;

    public float speed = 70f;
    public float explosionRadius = 0f;
    public int moneyPerEnemy;
    public GameObject impactEffect;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    
    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 4f);

        if(explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider col in cols)
        {
            if (col.CompareTag("Enemy"))
            {
                Damage(col.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        ShopManager.money += moneyPerEnemy;
        Destroy(enemy.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}