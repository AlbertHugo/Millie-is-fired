using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float forwardSpeed = 3f;
    public float laneChangeSpeed = 5f;
    public float laneOffset = 3f;

    public float enemyLife = 15f;

    private Transform player;
    private Rigidbody rb;
    private int currentLane = 0;

    void Start()
    {
        enemyLife = 15f;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // define lane inicial a partir da posição atual
        currentLane = Mathf.RoundToInt(transform.position.x / laneOffset);
    }

    void FixedUpdate()
    {
        // sempre corre pra frente
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.fixedDeltaTime;

        // detecta a lane do player
        int playerLane = Mathf.RoundToInt(player.position.x / laneOffset);

        // se for diferente, ajusta a lane alvo
        if (playerLane != currentLane)
        {
            currentLane = playerLane;
        }

        // move suavemente até a lane alvo
        float targetX = currentLane * laneOffset;
        float newX = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        Vector3 newPos = new Vector3(newX, rb.position.y, rb.position.z) + forwardMove;
        rb.MovePosition(newPos);
    }

    public void TakeDamage(float damage)
    {
        enemyLife -= damage;
        if (enemyLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}
