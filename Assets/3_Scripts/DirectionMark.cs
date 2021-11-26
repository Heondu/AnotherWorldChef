using UnityEngine;

public class DirectionMark : MonoBehaviour
{
    private Transform portal;
    private Transform player;
    [SerializeField] private Transform pivot;

    private void Start()
    {
        portal = FindObjectOfType<Portal>().transform;
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (portal != null && player != null)
        {
            Vector3 from = new Vector3(player.position.x, 0, player.position.z);
            Vector3 to = new Vector3(portal.position.x, 0, portal.position.z);
            Vector3 normal = (to - from).normalized;
            float angle = Mathf.Atan2(normal.z, normal.x) * Mathf.Rad2Deg;
            pivot.localRotation = Quaternion.Euler(0, 0, angle - 90);
            transform.position = new Vector3(player.position.x, player.position.y + 0.3f, player.position.z);
        }
    }
}
