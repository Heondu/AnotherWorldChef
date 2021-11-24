using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SettingManager settingPanel;
    [SerializeField] private InventoryUI inventoryPanel;

    private void Start()
    {
        inventoryPanel.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryPanel.gameObject.activeSelf)
            {
                inventoryPanel.Close();
            }
            else
            {
                if (settingPanel.gameObject.activeSelf)
                {
                    settingPanel.Cancel();
                }
                else
                {
                    settingPanel.Open();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            settingPanel.Cancel();
            if (inventoryPanel.gameObject.activeSelf)
            {
                inventoryPanel.Close();
                GameManager.Instance.IsStop = false;
            }
            else
            {
                inventoryPanel.Open();
                GameManager.Instance.IsStop = true;
            }
        }
    }
}
