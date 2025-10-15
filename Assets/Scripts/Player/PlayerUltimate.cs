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

        // Verifica se a ult est� pronta para ser ativada e se o cooldown j� passou
        if (haveUlt && cooldown <= Time.time && Input.GetKeyDown(KeyCode.X))
        {
            // Aplica cooldown de 10 segundos
            cooldown = Time.time + 10f;

            // Chama a fun��o para destruir inimigos e obst�culos
            PenUltimate();
        }
    }

    // Fun��o para destruir todos os inimigos e obst�culos
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

        // Destruir obst�culos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacleObj in obstacles)
        {
            Obstacle obstacle = obstacleObj.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.Destruction();  // Destr�i o obst�culo
            }
        }

        // Destruir objetos com tag "Insta Kill"
        GameObject[] instaKillObjects = GameObject.FindGameObjectsWithTag("Insta Kill");
        foreach (GameObject instaKillObj in instaKillObjects)
        {
            Obstacle instaKill = instaKillObj.GetComponent<Obstacle>();
            if (instaKill != null)
            {
                instaKill.Destruction();  // Destr�i o objeto
            }
        }
    }
}
