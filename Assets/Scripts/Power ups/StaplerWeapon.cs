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
            PlayerStats.baseATK = 1f;
            PlayerAttack.projectile = projectile;
            PlayerAttack.projectileIndex = 1;
            Destroy(gameObject);
        }
    }
}
