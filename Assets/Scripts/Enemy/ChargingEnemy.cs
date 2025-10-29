using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class ChargingEnemy : MonoBehaviour
{
    [Header("Velocidades")]
    private float laneOffset = 3f;            // Distância entre as pistas
    private float backwardSpeed;         // Velocidade para compensar o movimento do player
    private float chargeSpeed = 20f;          // Velocidade do avanço quando ataca
    private float laneChangeSpeed = 5f;       // (Opcional, caso queira suavizar o alinhamento)
    
    [Header("Comportamento")]
    private float idleTime = 10f;              // Tempo "parado" antes de avançar
    private bool isCharging = true;
    public AudioClip entrance;

    [Header("Vida")]
    public VisualEffect damageTaken;
    public float enemyLife = 30f;
    public int markCounter = 0;
    public float damagePerMark = 1f;        // quanto cada marcador causa por segundo
    private bool isTakingDot = false;       // controle da corrotina
    public AudioClip Death;


    [Header("Referências")]
    private Rigidbody rb;
    private PlayerStats playerStats;
    private int currentLane = 0;
    private float stateTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        RepeatableCode.PlaySound(entrance, gameObject.transform.position);

        // Define a lane inicial de acordo com a posição atual
        currentLane = Mathf.RoundToInt(transform.position.x / laneOffset);

        // Define o tempo para iniciar o ataque
        stateTimer = Time.time + idleTime;
    }

    void FixedUpdate()
    {
        // Movimento base (para compensar o movimento do player)
        backwardSpeed=playerStats.speed-1f;
        Debug.Log(backwardSpeed);

        // Durante a fase de "charge", ele se move para frente
        if (isCharging)
        {
            // Aplica movimento para frente
            Vector3 forwardMove = Vector3.forward * backwardSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + forwardMove);
            // Verifica se o ataque terminou
            if (Time.time >= stateTimer)
            {
                isCharging = false;
            }
        }
        else
        {
            // Começa o avanço
            Vector3 chargingMove = Vector3.back * chargeSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + chargingMove);
        }

        // Mantém posição na lane
        float targetX = currentLane * laneOffset;
        float smoothX = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage, int weaponIndex)
    {
        enemyLife -= damage;
        PlayDamageVFX();

        if (weaponIndex == 1)
        {
            markCounter += 1;

            // inicia dano por segundo quando atingir 3 marcadores
            if (markCounter >= 1 && !isTakingDot)
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

        while (enemyLife > 0 && markCounter >= 1)
        {
            float damageThisTick = damagePerMark * markCounter * 2;
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
