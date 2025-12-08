using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Velocidades")]
    public float laneChangeSpeed = 7f; // velocidade de troca de pista
    private float laneBaseSpeed = 7f;
    private float jumpForce = 8f;       // força do pulo
    private float jumpTimer = 0f;
    public Animator animator;

    [Header("Pistas")]
    public float laneOffset = 3f; // distância entre pistas
    private int currentLane = 0;  // -1 = esquerda, 0 = meio, 1 = direita

    private Rigidbody rb;
    private bool isGrounded = true; // checa se o jogador está no chão
    private Vector3 targetPosition;
    public static bool pauseMenuActive = false;
    private PlayerStats stats;
    public GameObject pauseMenu;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        laneChangeSpeed=laneBaseSpeed*stats.SPDBuff;
        // Movimento entre pistas
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > -1) // Impede que o jogador saia da pista pela esquerda
                currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 1) // impede que o jogador saia da pista pela direita
                currentLane++;
        }

        // Calcula posição alvo da pista
        targetPosition = new Vector3(currentLane * laneOffset, transform.position.y, transform.position.z);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded&&jumpTimer<=Time.time)
        {
            animator.Play("Armature_Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpTimer = Time.time+1f;
        }

        //impede o jogo de continuar pausado
        if (pauseMenuActive == false)
        {
            Time.timeScale = 1f;
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
