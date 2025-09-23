using UnityEngine;

public class StaplerWeapon : MonoBehaviour
{
    public GameObject projectile;
    private Vector3 spin = new Vector3(0, 10, 0);
    void Start()
    {
        Destroy(gameObject, 20f);
    }
    private void FixedUpdate()
    {
        transform.Rotate(spin);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats.baseATK = 3f;
            PlayerAttack.projectile = projectile;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
