using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static GameObject projectile; // prefab atribuído pelo power-up
    public Transform firePoint;          // onde o projétil nasce (pode ser null)
    public float fireRate = 0.5f;
    private float fireTimer;

    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate && projectile != null)
        {
            fireTimer = 0f;

            Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position + transform.forward * 1f;
            GameObject projObj = Instantiate(projectile, spawnPos, transform.rotation);

            // configura projétil (velocidade e dano)
            ProjectileMove pm = projObj.GetComponent<ProjectileMove>();
            if (pm != null)
            {
                float projSpeed = (stats != null) ? stats.speed * 2f : 6f;            // 2x speed do player
                pm.Initialize(projSpeed);
            }
        }
    }
}
