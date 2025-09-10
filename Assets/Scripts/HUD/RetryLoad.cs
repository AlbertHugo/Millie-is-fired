using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLoad : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Stage1");
        }
    }
}
