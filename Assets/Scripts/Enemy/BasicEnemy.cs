using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class BasicEnemy : MonoBehaviour
{
    public float forwardSpeed = 3f;
    public float laneChangeSpeed = 5f;
    public float laneOffset = 3f;
    public VisualEffect damageTaken;

    public float enemyLife = 15f;
    public int markCounter = 0;

    public float damagePerMark = 1f; // quanto cada marcador causa por segundo
    private bool isTakingDot = false; // controle da corrotina

    private Transform player;
    public PlayerStats playerStats;
    private Rigidbody rb;
    private int currentLane = 0;

    [Header("Efeitos sonoros")]
    private int entranceIndex = 0;
    public AudioClip entrance1;
    public AudioClip entrance2;
    public AudioClip Death;

    void Start()
    {
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
            damageTaken.gameObject.SetActive(false);
        enemyLife = 15f;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // define lane inicial a partir da posição atual
        currentLane = Mathf.RoundToInt(transform.position.x / laneOffset);
    }

    void FixedUpdate()
    {
        Debug.Log("Vida:" + enemyLife);
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

    public void TakeDamage(float damage, int weaponIndex)
    {
        enemyLife -= damage;
        PlayDamageVFX();

        if (weaponIndex == 1)
        {
            markCounter += 1;

            // inicia dano por segundo quando atingir 3 marcadores
            if (markCounter >= 3 && !isTakingDot)
            {
                StartCoroutine(ApplyMarkDamage());
            }
        }

        if (enemyLife <= 0)
        {
            RepeatableCode.PlaySound(Death, gameObject.transform.position);
            playerStats.score += 10;
            Destroy(gameObject);
        }
    }

    private void PlayDamageVFX()
    {
        damageTaken.gameObject.SetActive(true);
        VisualEffect vfxDamage = Instantiate(damageTaken, transform.position, Quaternion.identity);
        vfxDamage.Play();
        Destroy(vfxDamage.gameObject, 0.5f);
        damageTaken.gameObject.SetActive(false);
    }

    private IEnumerator ApplyMarkDamage()
    {
        isTakingDot = true;

        while (enemyLife > 0 && markCounter >= 3)
        {
            float damageThisTick = damagePerMark * markCounter;
            enemyLife -= damageThisTick;

            PlayDamageVFX();

            if (enemyLife <= 0)
            {
                playerStats.score += 100;
                Destroy(gameObject);
                yield break;
            }

            yield return new WaitForSeconds(1f); // aplica dano a cada 1 segundo
        }

        isTakingDot = false; // libera para poder reiniciar no futuro
    }
}