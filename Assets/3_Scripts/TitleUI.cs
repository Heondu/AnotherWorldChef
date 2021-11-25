using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private SettingManager settingPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
