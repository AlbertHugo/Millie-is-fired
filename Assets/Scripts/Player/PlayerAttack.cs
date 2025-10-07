using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static GameObject projectile;
    public static int projectileIndex;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireTimer;

    private PlayerStats stats;

    void Start()
    {
        projectile = null;
        projectileIndex = 0;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate && projectile != null)
        {
            fireTimer = 0f;

            // posição de spawn
            Vector3 spawnPos = (firePoint != null) 
                ? firePoint.position 
                : transform.position + transform.forward * 1f;
            spawnPos.y += 1;

            GameObject projObj = Instantiate(projectile, spawnPos, Quaternion.identity);

            // pega componente e inicializa
            ProjectileMove pm = projObj.GetComponent<ProjectileMove>();
            if (pm != null)
            {
                float projSpeed = (stats != null) ? stats.speed * 2f : 6f;
                Vector3 projDirection = transform.forward;
                pm.Initialize(projDirection, projSpeed, projectileIndex);
            }
        }
    }
}
