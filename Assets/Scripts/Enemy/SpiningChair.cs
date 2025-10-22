using UnityEngine;

public class SpiningChair : MonoBehaviour
{
    [Header("Movimento para frente")]
    public float forwardSpeed = 4f;       // Velocidade de avanço constante
    public float rotationSpeed = 180f;    // Velocidade de rotação (graus por segundo)

    [Header("Lanes")]
    public float laneChangeSpeed = 5f;    // Velocidade de troca de pista (quanto maior, mais rápido)
    public float laneOffset = 3f;         // Distância entre as pistas
    private int currentLane = 0;          // -1 = esquerda, 0 = meio, 1 = direita
    private int targetLane = 0;           // Lane que o obstáculo está indo

    private Rigidbody rb;
    private float laneVelocity = 0f;      // Controla a suavização (para o SmoothDamp)
    private float laneChangeTimer = 0f;   // Controle de tempo entre mudanças de lane
    public float laneChangeInterval = 2.5f; // Tempo entre trocas automáticas de lane

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Define uma lane inicial aleatória (-1, 0 ou 1)
        currentLane = Random.Range(-1, 2);
        targetLane = currentLane;

        // Garante que ele comece exatamente em uma lane válida
        Vector3 startPos = transform.position;
        startPos.x = currentLane * laneOffset;
        transform.position = startPos;

        laneChangeTimer = Time.time + laneChangeInterval;
    }

    void FixedUpdate()
    {
        // Movimento para frente
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.fixedDeltaTime;

        // Rotação contínua
        transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime, Space.World);

        // Alterna de lane automaticamente a cada X segundos
        if (Time.time >= laneChangeTimer)
        {
            ChangeLane();
            laneChangeTimer = Time.time + laneChangeInterval;
        }

        // Movimento suave entre lanes
        float targetX = targetLane * laneOffset;
        float smoothX = Mathf.SmoothDamp(rb.position.x, targetX, ref laneVelocity, 1f / laneChangeSpeed);

        Vector3 newPos = new Vector3(smoothX, rb.position.y, rb.position.z) + forwardMove;
        rb.MovePosition(newPos);
    }

    void ChangeLane()
    {
        // Escolhe uma nova lane diferente da atual
        int newLane;
        do
        {
            newLane = Random.Range(-1, 2);
        } while (newLane == targetLane); // Evita escolher a mesma lane

        targetLane = newLane;
    }
}
