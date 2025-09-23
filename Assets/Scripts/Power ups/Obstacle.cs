using UnityEngine;
using UnityEngine.VFX;

public class Obstacle : MonoBehaviour
{
    public VisualEffect damaged;
    public AudioClip breaked;

    private void Start()
    {
        damaged.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            damaged.gameObject.SetActive(true);
            VisualEffect vfxDamage = GameObject.Instantiate(damaged, gameObject.transform.position, Quaternion.identity);
            vfxDamage.Play();
            GameObject.Destroy(vfxDamage.gameObject, 1.5f);
            damaged.gameObject.SetActive(false);
            RepeatableCode.PlaySound(breaked, gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}