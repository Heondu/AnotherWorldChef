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

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Transform startPoint)
    {
        PlayerController playerController = Instantiate(player, startPoint.position, startPoint.rotation);
        playerController.Init(Instantiate(brainCamera));
        Instantiate(virtualCamera).Init(playerController.transform);
        Instantiate(canvas);
        Instantiate(eventSystem);
    }
}
