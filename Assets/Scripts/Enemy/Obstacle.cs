using UnityEngine;
using UnityEngine.VFX;

public class Obstacle : MonoBehaviour
{
    public GameObject damaged;
    public AudioClip breaked;
    private HitKill hitKill;

    private void Start()
    {
        hitKill = GetComponent<HitKill>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Destruction();
        }
    }
    public void Destruction()
    {
        damaged.gameObject.SetActive(true);
        GameObject vfxDamage = GameObject.Instantiate(damaged, gameObject.transform.position, Quaternion.identity);
        GameObject.Destroy(vfxDamage.gameObject, 0.5f);
        RepeatableCode.PlaySound(breaked, gameObject.transform.position);
        Destroy(gameObject);
        if (hitKill != null)
        {
            hitKill.SelfDestruct();
        }
    }
}