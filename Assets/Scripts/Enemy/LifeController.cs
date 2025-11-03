using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class LifeController : MonoBehaviour
{

    public AudioClip Death;
    public int markCounter = 0;
    public VisualEffect damageTaken;
    public PlayerStats playerStats;
    public float damagePerMark = 1f; // quanto cada marcador causa por segundo
    private bool isTakingDot = false; // controle da corrotina
    public float enemyLife;

    public void Start()
    {
        damageTaken.gameObject.SetActive(false);
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        damageTaken.gameObject.SetActive(false);
    }
    public void SetLife(float life)
    {
        enemyLife = life;
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
