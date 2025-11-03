using UnityEngine;
using UnityEngine.VFX;

public class BasicEnemy : MonoBehaviour
{
    [Header("Movimento")]
    public float forwardSpeed = 3f;
    public float laneChangeSpeed = 5f;
    public float laneOffset = 3f;

    [Header("Referências")]
    private float enemyLife = 15f;
    LifeController lifeController;
    private Transform player;
    private Rigidbody rb;
    private int currentLane = 0;

    [Header("Efeitos sonoros")]
    private int entranceIndex = 0;
    public AudioClip entrance1;
    public AudioClip entrance2;
    

    void Start()
    {
        lifeController = GetComponent<LifeController>();
        lifeController.SetLife(enemyLife);
        entranceIndex = Random.Range(0, 3);//seleciona qual o efeito sonoro que será tocado na entrada
        //toca o som na entrada, ou não toca nada
        if (entranceIndex >= 1&&entranceIndex<2)
        {
            RepeatableCode.PlaySound(entrance1, gameObject.transform.position);
        }
        else if(entranceIndex >= 0&&entranceIndex<1)
        {
            RepeatableCode.PlaySound(entrance2 , gameObject.transform.position);
        }
        
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
}