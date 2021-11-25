using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance != null)
                    DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }
    private static GameManager instance;

    [SerializeField] private PlayerController player;
    [SerializeField] private Camera brainCamera;
    [SerializeField] private CameraController virtualCamera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject eventSystem;

    public bool IsStop;
    private int currentSceneIndex = 0;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
#if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadNextScene(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) LoadNextScene(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) LoadNextScene(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) LoadNextScene(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) LoadNextScene(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) LoadNextScene(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7)) LoadNextScene(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8)) LoadNextScene(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9)) LoadNextScene(8);
        else if (Input.GetKeyDown(KeyCode.Alpha0)) LoadNextScene(9);
        else if (Input.GetKeyDown(KeyCode.Minus)) LoadNextScene(10);
#endif
    }

    public void LoadNextScene(int sceneIndex)
    {
        currentSceneIndex = sceneIndex;

        switch (sceneIndex)
        {
            case 0: LoadingSceneManager.LoadScene("01_Beach0"); break;
            case 1: LoadingSceneManager.LoadScene("01_Beach1"); break;
            case 2: LoadingSceneManager.LoadScene("01_Beach2"); break;
            case 3: LoadingSceneManager.LoadScene("02_Castle0"); break;
            case 4: LoadingSceneManager.LoadScene("02_Castle1"); break;
            case 5: LoadingSceneManager.LoadScene("02_Castle2"); break;
            case 6: LoadingSceneManager.LoadScene("02_Castle3"); break;
            case 7: LoadingSceneManager.LoadScene("03_Dungeon0"); break;
            case 8: LoadingSceneManager.LoadScene("03_Dungeon1"); break;
            case 9: LoadingSceneManager.LoadScene("03_Dungeon2"); break;
            case 10: LoadingSceneManager.LoadScene("TitleScene"); break;
        }
    }

    public void LoadNextScene()
    {
        LoadNextScene(++currentSceneIndex);
    }

    public void Init(Transform startPoint, Vector3 cameraOffset)
    {
        PlayerController playerController = Instantiate(player, startPoint.position, startPoint.rotation);
        playerController.Init(Instantiate(brainCamera));
        Instantiate(virtualCamera).Init(playerController.transform, cameraOffset);
        Instantiate(canvas);
        Instantiate(eventSystem);
    }
}
