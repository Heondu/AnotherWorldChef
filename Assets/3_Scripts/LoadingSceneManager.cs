using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private AsyncOperation op;
    //[SerializeField] Slider progressBar;

    private void Start()
    {
        //StartCoroutine(LoadScene());
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
    }

    public static void LoadScene(string sceneName)
    { 
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadNextScene()
    {
        //SceneManager.LoadScene(nextScene);
        op.allowSceneActivation = true;
    }
}
