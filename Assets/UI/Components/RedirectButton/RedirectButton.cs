using UnityEngine;

public class RedirectButton : MonoBehaviour
{
    public void RedirectTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
