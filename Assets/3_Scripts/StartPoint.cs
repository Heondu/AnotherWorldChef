using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset = new Vector3(16, 32, 16);
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager, Vector3.zero, Quaternion.identity);
        }

        GameManager.Instance.Init(transform, cameraOffset);

        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + cameraOffset.normalized * 2);
    }
}
