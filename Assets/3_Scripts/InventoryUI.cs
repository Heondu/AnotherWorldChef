using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform shortcutParent;
    public Transform skillsParent;
    public Transform shortcutHUDParent;

    private InventorySlot[] shortcutSlots;
    private InventorySlot[] skillSlots;
    private InventorySlot[] shortcutHUDSlot;

    public void Init()
    {
        Inventory.Instance.onItemChanged += UpdateUI;

        shortcutSlots = shortcutParent.GetComponentsInChildren<InventorySlot>();
        skillSlots = skillsParent.GetComponentsInChildren<InventorySlot>();
        shortcutHUDSlot = shortcutHUDParent.GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            shortcutSlots[i].Init(i, transform);
        }
        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].Init(i, transform);
        }
        for (int i = 0; i < shortcutHUDSlot.Length; i++)
        {
            shortcutHUDSlot[i].Init(i, transform);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (Inventory.Instance.skills[i] != null)
            {
                skillSlots[i].AddSkill(Inventory.Instance.skills[i]);
            }
            else
            {
                skillSlots[i].ClearSlot();
            }
        }

        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            if (Inventory.Instance.shortcuts[i] != null)
            {
                shortcutSlots[i].AddSkill(Inventory.Instance.shortcuts[i]);
            }
            else
            {
                shortcutSlots[i].ClearSlot();
            }
        }

        for (int i = 0; i < shortcutHUDSlot.Length; i++)
        {
            if (Inventory.Instance.shortcuts[i] != null)
            {
                shortcutHUDSlot[i].AddSkill(Inventory.Instance.shortcuts[i]);
            }
            else
            {
                shortcutHUDSlot[i].ClearSlot();
            }
        }
    }

    public void Open()
    {
        Time.timeScale = 0;
        if (GameManager.Instance != null)
            GameManager.Instance.IsStop = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Time.timeScale = 1;
        if (GameManager.Instance != null)
            GameManager.Instance.IsStop = false;
        gameObject.SetActive(false);
    }
}
