using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Código completamente provisório, apenas para testes
    public GameObject projectile;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        HandleShooting();
    }

    void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject selectedShot = projectile;

            nextFireTime = Time.time + fireRate;
            Vector3 spawnPosition = transform.position + transform.forward * 1.0f;
            Instantiate(selectedShot, spawnPosition, transform.rotation);
        }
    }
}
