using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Init(transform);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
