using System;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    public float lifetime = 5f;
    private int selfIndex;

    public void Initialize(Vector3 dir, float spd, int projectile)
    {
        direction = dir.normalized; // garante direção unitária
        speed = spd;
        selfIndex = projectile;

        Destroy(gameObject, lifetime); // autodestruir
    }

    void Update()
    {
        // Move constantemente na direção definida
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BasicEnemy enemy = other.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(PlayerStats.damage, selfIndex);
            }
            Destroy(gameObject);
        }else if (selfIndex==2&&other.gameObject.tag == "Insta Kill")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            obstacle.Destruction();
        }
        else if (other.gameObject.tag!="Player")
        {
            Debug.Log("Colisão detectada");
            Destroy(gameObject);
        }
    }
}
