using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Velocidades")]
    public float forwardSpeed = 10f;   // velocidade para frente
    public float horizontalSpeed = 5f; // velocidade lateral

    [Header("Limites da pista")]
    public float minX = -5f; // limite esquerdo
    public float maxX = 5f;  // limite direito

    private Rigidbody rb;
    private float horizontalInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; 
        // congela rotação para não tombar
    }

    void Update()
    {
        // Captura do input horizontal (A/D ou setas)
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // Movimento automático para frente
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.fixedDeltaTime;

        // Movimento horizontal
        float moveX = horizontalInput * horizontalSpeed * Time.fixedDeltaTime;
        Vector3 horizontalMove = new Vector3(moveX, 0f, 0f);

        // Calcula nova posição
        Vector3 newPos = rb.position + forwardMove + horizontalMove;

        // Restringe dentro dos limites
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        // Aplica movimento via física
        rb.MovePosition(newPos);
    }
}
