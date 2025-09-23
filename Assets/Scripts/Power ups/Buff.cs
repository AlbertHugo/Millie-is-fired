using UnityEngine;

public class Buff : MonoBehaviour
{
    private Vector3 spin = new Vector3(0, 10, 0);
    void Start()
    {
        Destroy(gameObject, 20f);
    }
    private void FixedUpdate()
    {
        transform.Rotate(spin);
    }
}
