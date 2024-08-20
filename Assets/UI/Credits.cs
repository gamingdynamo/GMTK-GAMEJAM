using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Load the scene at index 0
            SceneManager.LoadScene(0);
        }
    }
}
