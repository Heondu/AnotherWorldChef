using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private Skill skill;

    private void Start()
    {
        skill = GetComponent<Skill>();
    }

    private void OnTriggerEnter(Collider other)
    {
        skill.Attack(other.gameObject);
    }
}
