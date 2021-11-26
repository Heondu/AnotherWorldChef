using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private AsyncOperation op;

    private void Start()
    {
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadNextScene();
        }
    }

    public static void LoadScene(string sceneName)
    { 
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadNextScene()
    {
        op.allowSceneActivation = true;
    }

    public static void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
