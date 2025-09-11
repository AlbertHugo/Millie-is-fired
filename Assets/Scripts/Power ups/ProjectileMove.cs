using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMove : MonoBehaviour
{
    public float lifetime = 5f;
    private float speed = 10f;

    private Rigidbody rb;
    private bool initialized = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Deve ser chamado logo após o Instantiate para configurar velocidade e dano.
    /// </summary>
    public void Initialize(float projectileSpeed)
    {
        speed = projectileSpeed;

        // aplica velocidade imediatamente
        rb.velocity = transform.forward * speed;

        // garantir autodestruição para não acumular objetos
        Destroy(gameObject, lifetime);

        initialized = true;
    }

    // fallback: se por alguma razão Initialize não foi chamado, tenta buscar o Player e usar speed atual
    void Start()
    {
        if (!initialized)
        {
            PlayerStats ps = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStats>();
            float fallbackSpeed = (ps != null) ? ps.speed * 2f : speed;
            rb.velocity = transform.forward * fallbackSpeed;
            Destroy(gameObject, lifetime);
            initialized = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BasicEnemy enemy = other.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(PlayerStats.damage);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            // colidiu com cenário ou outros — destrói (ajuste conforme necessidade)
            Destroy(gameObject);
        }
    }
}
