using System;
using UnityEngine;

public class PlayerUltimate : MonoBehaviour
{
    public static bool haveUlt;
    public static int ultIndex;
    public GameObject ultIcon1;
    private float cooldown = 0f;

    void Start()
    {
        haveUlt = false;
        ultIcon1.SetActive(false);
        ultIndex = 0;
    }

    void Update()
    {
        if (haveUlt)
        {
            ultIcon1.SetActive(true);
        }

        // Verifica se a ult está pronta para ser ativada e se o cooldown já passou
        if (haveUlt && cooldown <= Time.time && Input.GetKeyDown(KeyCode.X))
        {
            // Aplica cooldown de 10 segundos
            cooldown = Time.time + 10f;

            // Chama a função para destruir inimigos e obstáculos
            PenUltimate();
        }
    }

    // Função para destruir todos os inimigos e obstáculos
    void PenUltimate()
    {
        // Destruir inimigos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemies)
        {
            BasicEnemy enemy = enemyObj.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(100, 1);  // Dano fatal
            }
        }

        // Destruir obstáculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacleObj in obstacles)
        {
            Obstacle obstacle = obstacleObj.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.Destruction();  // Destrói o obstáculo
            }
        }

        // Destruir objetos com tag "Insta Kill"
        GameObject[] instaKillObjects = GameObject.FindGameObjectsWithTag("Insta Kill");
        foreach (GameObject instaKillObj in instaKillObjects)
        {
            Obstacle instaKill = instaKillObj.GetComponent<Obstacle>();
            if (instaKill != null)
            {
                instaKill.Destruction();  // Destrói o objeto
            }
        }
    }
}
