using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nextSceneName != "")
        {
            sceneLoader.LoadScene(nextSceneName);
        }
    }
}
