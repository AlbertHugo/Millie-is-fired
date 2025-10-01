using UnityEngine;

public class RulerWeapon : MonoBehaviour
{
    public GameObject self;
    private Vector3 spin = new Vector3 (0, 10, 0);
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
            PlayerStats.baseATK = 7f;
            Debug.Log("Atribuido");
            PlayerAttack.projectile = self;
            PlayerAttack.projectileIndex = 0;
            Destroy(gameObject);
        }
    }
}
