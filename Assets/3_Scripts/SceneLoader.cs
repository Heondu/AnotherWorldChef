using UnityEngine;
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string nextSceneName)
    {
        LoadingSceneManager.LoadScene(nextSceneName);
    }
}
