using UnityEngine;
using UnityEngine.UI;

public class CoolTimeViewer : MonoBehaviour
{
    private Image image;
    private SkillShooting skillShooting;
    [SerializeField] private int index;

    private void Start()
    {
        image = GetComponent<Image>();
        skillShooting = FindObjectOfType<SkillShooting>();
    }

    private void Update()
    {
        if (Inventory.Instance.shortcuts[index] == null)
        {
            image.fillAmount = 0;
            return;
        }
        else if (skillShooting.skillTimers.ContainsKey(Inventory.Instance.shortcuts[index]) == false) return;

        image.fillAmount = skillShooting.skillTimers[Inventory.Instance.shortcuts[index]].GetCoolTimeRatio();
    }
}
