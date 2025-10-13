using UnityEngine;

public class Pen : MonoBehaviour
{
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
            PlayerUltimate.haveUlt = true;
            PlayerUltimate.ultIndex = 0;
            Destroy(gameObject);
        }
    }
}