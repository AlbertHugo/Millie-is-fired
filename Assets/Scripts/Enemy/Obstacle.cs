using UnityEngine;
using UnityEngine.VFX;

public class Obstacle : MonoBehaviour
{
    public VisualEffect damaged;
    public AudioClip breaked;
    private HitKill hitKill;

    private void Start()
    {
        damaged.gameObject.SetActive(false);
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
        VisualEffect vfxDamage = GameObject.Instantiate(damaged, gameObject.transform.position, Quaternion.identity);
        vfxDamage.Play();
        GameObject.Destroy(vfxDamage.gameObject, 1.5f);
        damaged.gameObject.SetActive(false);
        RepeatableCode.PlaySound(breaked, gameObject.transform.position);
        Destroy(gameObject);
        if (hitKill != null)
        {
            hitKill.SelfDestruct();
        }
    }
}