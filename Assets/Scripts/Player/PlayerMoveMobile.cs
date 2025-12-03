using UnityEngine;

public class PlayerMoveMobile : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneChangeSpeed = 8f;  // velocidade de troca de pista base
    private float laneBaseSpeed = 8f;
    private float jumpForce = 8f;       // força do pulo
    private float jumpTimer = 0f;

    [Header("Pistas")]
    public float laneOffset = 3f;       // distância entre as pistas
    private int currentLane = 0;        // -1 = esquerda, 0 = meio, 1 = direita

    [Header("Menu de Pausa")]
    public static bool pauseMenuActive = false;
    public GameObject pauseMenu;

    private Rigidbody rb;
    private bool isGrounded = true;     // checa se o jogador está no chão
    private Vector3 targetPosition;
    private PlayerStats stats;

    // Controle de toque
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Aplica multiplicador de buff de velocidade
        laneChangeSpeed = laneBaseSpeed * stats.SPDBuff;

        //CONTROLE POR TOQUE
        if (Input.touchCount > 0)
        {
            
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

            // Início do toque
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = false;
            }

            // Arrasto detectado
            if (touch.phase == TouchPhase.Moved)
            {
                float swipeDistance = touch.position.x - touchStartPos.x;

                if (!isSwiping && Mathf.Abs(swipeDistance) > 50)
                {
                    isSwiping = true;

                    if (swipeDistance < 0 && currentLane > -1)
                        currentLane--; // Esquerda
                    else if (swipeDistance > 0 && currentLane < 1)
                        currentLane++; // Direita
                }
            }

            // Fim do toque
            if (touch.phase == TouchPhase.Ended)
            {
                float verticalSwipe = touch.position.y - touchStartPos.y;

                // Só considera pulo se NÃO foi um swipe
                if (!isSwiping && Mathf.Abs(verticalSwipe) < 100)
                {
                    if (isGrounded && jumpTimer <= Time.time)
                    {
                        jumpRequested = true;
                        jumpTimer = Time.time + 1f; // tempo mínimo entre pulos
                    }
                }
            }
        }

        //PAUSE MENU (por botão físico ou UI)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        //Calcula posição alvo
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

        // Movimento suave lateral
        Vector3 newPos = new Vector3(targetPosition.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, newPos, laneChangeSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu == null) return;

        if (!pauseMenuActive)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            pauseMenuActive = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            pauseMenuActive = false;
        }
    }
}
