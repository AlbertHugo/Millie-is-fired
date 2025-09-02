using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Velocidades")]
    public float forwardSpeed = 3f;    // velocidade para frente
    public float laneChangeSpeed = 8f; // velocidade de troca de pista
    public float jumpForce = 7f;       // força do pulo

    [Header("Pistas")]
    public float laneOffset = 3f; // distância entre pistas
    private int currentLane = 0;  // -1 = esquerda, 0 = meio, 1 = direita

    private Rigidbody rb;
    private bool isGrounded = true; // checa se o jogador está no chão
    private Vector3 targetPosition;
    private PlayerStats stats;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        targetPosition = transform.position;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Movimento entre pistas
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > -1) // limite na esquerda
                currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 1) // limite na direita
                currentLane++;
        }

        // Calcula posição alvo da pista
        targetPosition = new Vector3(currentLane * laneOffset, transform.position.y, transform.position.z);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Movimento automático para frente
        Vector3 forwardMove = Vector3.forward * stats.speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // Movimento suave para a pista
        Vector3 newPos = new Vector3(targetPosition.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, newPos, laneChangeSpeed * Time.fixedDeltaTime);
    }

    // Detecta se está no chão (precisa de um collider no "chão")
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
