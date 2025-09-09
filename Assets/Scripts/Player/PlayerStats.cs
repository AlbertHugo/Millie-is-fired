using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayerStats : MonoBehaviour
{
    [Header("Referências")]
    public AudioClip damageTook;
    [Header("Atributos do Jogador")]
    public float life = 3f;
    private float baseLife = 3f;
    public float damage = 3f; 
    public float speed = 4f;
    private float baseSpeed = 4f;

    [Header("Buffs dos Atributos")]
    public float HPBuff = 0f;
    public float ATKBuff = 1f;
    public float SPDBuff = 1f;

    // Ainda preciso programar melhor a aplicação deles
    [Header("Pontuação e distância")]
    public float score = 0f;

    private void FixedUpdate()
    {
        //código para medir a distância
        score = gameObject.gameObject.transform.position.z;
        //código de aceleração
        if (speed <= 20)
        {
            baseSpeed += gameObject.transform.position.z / 5000;
        }
        //velocidade é multiplicada pelo buff de velocidade atual
        speed=baseSpeed*SPDBuff;
        //vida soma com o buff ao invés de multiplicar, para ficar sempre em valores inteiros
        life = baseLife + HPBuff;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Dano de objeto básico
        if (other.gameObject.tag == "Obstacle")
        {
            TakeDamage(1);
        //Dano de morte imediata
        }else if (other.gameObject.tag == "Insta Kill")
        {
            TakeDamage(life);
        }

        //Colisões com power ups
        if (other.gameObject.tag == "SpeedBuff")
        {
            if (SPDBuff < 3)
            {
                SPDBuff +=0.1f;
            }
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "DamageBuff")
        {
            ATKBuff += 0.1f;
            Destroy(other.gameObject);
        }
        else if((other.gameObject.tag =="HPBuff"))
        {
            HPBuff += 1f;
            Destroy(other.gameObject);
        }
    }
    //código de dano básico
    public void TakeDamage(float damage)
    {
        life -= damage;
        Debug.Log(life);
        if (life > 0)
        {
           baseSpeed = baseSpeed / 2;
            RepeatableCode.PlaySound(damageTook, gameObject.transform.position);
            StartCoroutine(CameraShake.instance.Shake(0.3f, 0.3f));
        }
        else
        {
            SceneManager.LoadScene("stage1");
        }
    }
}
