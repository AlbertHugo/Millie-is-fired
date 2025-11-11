using UnityEngine;
using UnityEngine.VFX;

public class HitKill : MonoBehaviour
{
    public VisualEffect aura;
    VisualEffect vfxAura;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aura.gameObject.SetActive(true);
        vfxAura = GameObject.Instantiate(aura, gameObject.transform.position, Quaternion.identity);
        vfxAura.Play();
        aura.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void SelfDestruct()
    {
       Destroy(vfxAura);
    }
}
