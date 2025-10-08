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
        //verifica se está no cooldown e com a ult, e então destrói tudo com o pressionar de X
        if (haveUlt && cooldown <= Time.time&&Input.GetKeyDown(KeyCode.X))
        {
            cooldown = Time.time + 10f;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i <= enemies.Length; i++)
            {
                BasicEnemy enemy = enemies[i].GetComponent<BasicEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(100, 1);
                }
            }
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            for (int i = 0; i <= obstacles.Length; i++)
            {
                Obstacle enemy = obstacles[i].GetComponent<Obstacle>();
                if (enemy != null)
                {
                    enemy.Destruction();
                }
            }
            GameObject[] instaKill = GameObject.FindGameObjectsWithTag("Insta Kill");
            for (int i = 0; i <= instaKill.Length; i++)
            {
                Obstacle enemy = instaKill[i].GetComponent<Obstacle>();
                if (enemy != null)
                {
                    enemy.Destruction();
                }
            }
        }
    }
}
