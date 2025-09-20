using UnityEngine;

public class PlayerMoveMobile : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneChangeSpeed = 8f;  // Velocidade de troca de pista
    public float jumpForce = 5f;        // Força do pulo

    [Header("Pistas")]
    public float laneOffset = 3f;       // Distância entre as pistas
    private int currentLane = 0;        // -1 = esquerda, 0 = meio, 1 = direita

    private Rigidbody rb;
    private bool isGrounded = true;     // Checa se o jogador está no chão
    private Vector3 targetPosition;
    private PlayerStats stats;

    // Para controle de toque
    private Vector2 touchStartPos;      // Posição inicial do toque
    private bool isSwiping = false;     // Flag para saber se está arrastando
    private bool jumpRequested = false; // Flag para saber se o pulo foi solicitado

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Detectando toque na tela
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = false;
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                // Detecta o arrasto para a esquerda ou direita
                float swipeDistance = touch.position.x - touchStartPos.x;

                if (!isSwiping && Mathf.Abs(swipeDistance) > 50) // Caso o arrasto seja significativo
                {
                    isSwiping = true;
                    if (swipeDistance < 0) // Arrasto para a esquerda
                    {
                        if (currentLane > -1) // Impede que o jogador saia da pista pela esquerda
                            currentLane--;
                    }
                    else if (swipeDistance > 0) // Arrasto para a direita
                    {
                        if (currentLane < 1) // Impede que o jogador saia da pista pela direita
                            currentLane++;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // Verifica se o toque foi para pular
                if (Mathf.Abs(touch.position.y - touchStartPos.y) < 100) // Verifica um toque simples
                {
                    if (isGrounded)
                    {
                        jumpRequested = true;
                    }
                }
            }
        }

        // Calcula a posição alvo da pista
        targetPosition = new Vector3(currentLane * laneOffset, transform.position.y, transform.position.z);

        // Pulo
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpRequested = false;
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

    void OnCollisionEnter(Collision collision)
    {
        // Detecta se está no chão
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
