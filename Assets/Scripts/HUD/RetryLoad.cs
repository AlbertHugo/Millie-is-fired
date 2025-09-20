using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLoad : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 ||Input.anyKey)
        {
            SceneManager.LoadScene("Stage1");
        }
    }
}
