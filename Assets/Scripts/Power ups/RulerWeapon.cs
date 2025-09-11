using UnityEngine;

public class RulerWeapon : MonoBehaviour
{
    public GameObject self;
    void Start()
    {
        Destroy(gameObject, 10f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats.baseATK = 10f;
            PlayerAttack.projectile = self;
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "enemy")
        {
            BasicEnemy enemy = other.GetComponent<BasicEnemy>();
            enemy.TakeDamage(PlayerStats.damage);
            Destroy(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
}
