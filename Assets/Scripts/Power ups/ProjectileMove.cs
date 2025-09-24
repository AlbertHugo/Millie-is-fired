using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    private float speed;

    private float baseSpeed = 20f;

    private float actualSpeed;
    private Vector3 direction;
    public float lifetime = 5f;

    public void Initialize(Vector3 dir, float spd)
    {
        direction = dir.normalized; // garante direção unitária
        speed = spd;

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
                enemy.TakeDamage(PlayerStats.damage, 0);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.tag!="Player")
        {
            Destroy(gameObject);
        }
    }
}
