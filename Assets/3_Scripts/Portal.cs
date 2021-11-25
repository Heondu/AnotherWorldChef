using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private bool isBossPortal = false;
    private ParticleSystem particle;
    private BoxCollider boxCollider;

    private Enemy[] enemys;

    private void Start()
    {
        if (isBossPortal)
        {
            enemys = FindObjectsOfType<Enemy>();
            particle = GetComponentInChildren<ParticleSystem>();
            particle.gameObject.SetActive(false);
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (isBossPortal)
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                if (enemys[i] != null) return;
            }

            particle.gameObject.SetActive(true);
            boxCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoadNextScene();
        }
    }
}
