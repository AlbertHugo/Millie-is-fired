using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Atributos do Jogador")]
    public float life = 3f;
    public float damage = 3f; 
    public float speed = 3f;

    // Ainda preciso programar melhor a aplicação deles
    [Header("Pontuação e distância")]
    public float score = 0f;

    private void FixedUpdate()
    {
        //código para medir a distância
        score += gameObject.gameObject.transform.position.z;
        //código de aceleração
        if (speed <= 20)
        {
            speed += gameObject.transform.position.z / 5000;
        }
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
    }
    //código de dano básico
    public void TakeDamage(float damage)
    {
        life -= damage;
        Debug.Log(life);
        if (life > 0)
        {
           speed = speed / 2;
        }
        else
        {
            SceneManager.LoadScene("stage1");
        }
    }
}
