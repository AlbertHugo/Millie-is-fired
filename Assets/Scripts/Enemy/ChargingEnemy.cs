using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneOffset = 3f;            // Distância entre as pistas
    public float backwardSpeed = 0f;         // Velocidade para compensar o movimento do player
    public float chargeSpeed = 10f;          // Velocidade do avanço quando ataca
    public float laneChangeSpeed = 5f;       // (Opcional, caso queira suavizar o alinhamento)
    
    [Header("Comportamento")]
    public float idleTime = 2f;              // Tempo "parado" antes de avançar
    public float chargeDuration = 2.5f;      // Tempo de duração do ataque
    private bool isCharging = false;
    private bool hasCharged = false;

    private Rigidbody rb;
    private PlayerStats playerStats;         // Para ler a velocidade do jogador
    private int currentLane = 0;
    private float stateTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        // Define a lane inicial de acordo com a posição atual
        currentLane = Mathf.RoundToInt(transform.position.x / laneOffset);

        // Define o tempo para iniciar o ataque
        stateTimer = Time.time + idleTime;
    }

    void FixedUpdate()
    {
        // Movimento base (para compensar o movimento do player)
        float zMovement = -playerStats.speed * Time.fixedDeltaTime;

        // Durante a fase de "charge", ele se move para frente
        if (isCharging)
        {
            zMovement += chargeSpeed * Time.fixedDeltaTime;

            // Verifica se o ataque terminou
            if (Time.time >= stateTimer)
            {
                isCharging = false;
                hasCharged = true; // evita que ataque novamente
            }
        }
        else if (!hasCharged && Time.time >= stateTimer)
        {
            // Começa o avanço
            isCharging = true;
            stateTimer = Time.time + chargeDuration;
        }

        // Mantém posição na lane
        float targetX = currentLane * laneOffset;
        float smoothX = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        // Aplica movimento final
        Vector3 move = new Vector3(smoothX, rb.position.y, rb.position.z + zMovement);
        rb.MovePosition(move);
    }
}
