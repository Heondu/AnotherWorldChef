using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform shortcutParent;
    public Transform skillsParent;
    public Transform shortcutHUDParent;

    private InventorySlot[] shortcutSlots;
    private InventorySlot[] skillSlots;
    private InventorySlot[] shortcutHUDSlot;

    private Status playerStatus;

    public void Init()
    {
        Inventory.Instance.onItemChanged = null;
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

        playerStatus = FindObjectOfType<PlayerController>().GetComponent<Status>();
        Inventory.Instance.onItemChanged += UpdateShortcut;
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Instance.isFirstInit)
            {
                Inventory.Instance.shortcuts[i] = playerStatus.skills[i + 1];
            }
            else
            {
                playerStatus.skills[i + 1] = Inventory.Instance.shortcuts[i];
            }
        }
        Inventory.Instance.onItemChanged.Invoke();
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

    private void UpdateShortcut()
    {
        for (int i = 0; i < Inventory.Instance.shortcuts.Length; i++)
        {
            playerStatus.skills[i + 1] = Inventory.Instance.shortcuts[i];
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
