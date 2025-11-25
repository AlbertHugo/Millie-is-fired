using UnityEngine;

public class Scissor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerUltimate.haveUlt = true;
            PlayerUltimate.ultIndex = 2;
            Destroy(gameObject);
        }
    }
}
