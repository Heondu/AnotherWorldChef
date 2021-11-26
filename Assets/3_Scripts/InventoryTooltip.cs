using UnityEngine;
using UnityEngine.UI;

public class InventoryTooltip : MonoBehaviour
{
    [SerializeField] private new Text name;
    [SerializeField] private Text damage;
    [SerializeField] private Text cooltime;
    [SerializeField] private Text desc;

    private Status playerStatus;

    public void Init(Skill skill, Transform inventoryPanel)
    {
        if (playerStatus == null)
        {
            playerStatus = FindObjectOfType<PlayerController>().GetComponent<Status>();
        }

        name.text = skill.skillName;
        damage.text = $"데미지 : {Mathf.RoundToInt(playerStatus.Damage * skill.skillDamageMult)}";
        cooltime.text = $"쿨타임 : {skill.coolTime}";
        desc.text = skill.skillDesc;
        transform.SetParent(inventoryPanel);
    }
}
