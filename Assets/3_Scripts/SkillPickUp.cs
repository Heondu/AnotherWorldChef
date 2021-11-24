using UnityEngine;

public class SkillPickUp : MonoBehaviour
{
    [SerializeField] private Skill[] skills;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Skill skill = skills[Random.Range(0, skills.Length)];
            bool wasPickedUp = Inventory.Instance.Add(skill);

            if (wasPickedUp)
                Destroy(gameObject);
        }
    }
}
