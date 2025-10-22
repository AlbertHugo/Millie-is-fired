using UnityEngine;

public class SpiningChair : MonoBehaviour
{
    [Header("Movimento para frente")]
    public float rotationSpeed = 180f;    // Velocidade de rotação
    private float forwardSpeed = 3;

    [Header("Lanes")]
    private float laneChangeSpeed = 10f;    // Velocidade de troca de pista 
    public float laneOffset = 3f;         // Distância entre as pistas
    private int currentLane = 0;          // -1 = esquerda, 0 = meio, 1 = direita
    private int targetLane = 0;           // Lane que o obstáculo está indo

    private Rigidbody rb;
    private float laneVelocity = 0f;      // Controla a suavização
    private float laneChangeTimer = 0f;   // Controle de tempo entre mudanças de lane
    private float laneChangeInterval = 0.5f; // Tempo entre trocas automáticas de lane
    float targetX;
    float newX;


    void Start()
    {
        currentLane = Mathf.RoundToInt(transform.position.x / laneOffset);
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 forwardMove = Vector3.back * forwardSpeed * Time.fixedDeltaTime;
        // Rotação contínua
        transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime, Space.World);

        //movimento
        if(Time.time>laneChangeTimer)
        {
            ChangeLane();
            laneChangeTimer = Time.time+laneChangeInterval;
        }

        currentLane = targetLane;
        float targetX = currentLane * laneOffset;
        float newX = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        Vector3 newPos = new Vector3(newX, transform.position.y, transform.position.z-0.1f);
        transform.position = newPos;
    }

    void ChangeLane()
    {
        // Escolhe uma nova lane diferente da atual
        int newLane;
        
        newLane = Random.Range(-1, 1);

        targetLane = newLane;
    }
}
