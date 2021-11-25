using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Image seFill;
    [SerializeField] private Text seText;
    [SerializeField] private Image bgmFill;
    [SerializeField] private Text bgmText;

    private float originSeVolume = 1;
    private float originBgmVolume = 1;
    private float seVolume = 1;
    private float bgmVolume = 1;

    private void OnEnable()
    {
        originSeVolume = SoundManager.soundManager.GetSEVolume();
        originBgmVolume = SoundManager.soundManager.GetBGMVolume();
        UpdateSEVolume(originSeVolume);
        UpdateBGMVolume(originBgmVolume);
    }

    public void SetSEVolume(float value)
    {
        seVolume = Mathf.Clamp(seVolume + value, 0, 1);
        UpdateSEVolume(seVolume);
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp(bgmVolume + value, 0, 1);
        UpdateBGMVolume(bgmVolume);
    }

    private void UpdateSEVolume(float value)
    {
        SoundManager.soundManager.SetSEVolume(value);
        seFill.fillAmount = value;
        seText.text = $"{Mathf.RoundToInt(value * 100)} / 100";
    }

    private void UpdateBGMVolume(float value)
    {
        SoundManager.soundManager.SetBGMVolume(value);
        bgmFill.fillAmount = value;
        bgmText.text = $"{Mathf.RoundToInt(value * 100)} / 100";
    }

    public void Quit()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Open()
    {
        Time.timeScale = 0;
        if (GameManager.Instance != null)
            GameManager.Instance.IsStop = true;
        gameObject.SetActive(true);
    }

    private void Close()
    {
        Time.timeScale = 1;
        if (GameManager.Instance != null)
            GameManager.Instance.IsStop = false;
        gameObject.SetActive(false);
    }

    public void Accept()
    {
        Close();
    }

    public void Cancel()
    {
        if (gameObject.activeSelf == false) return;

        seVolume = originSeVolume;
        bgmVolume = originBgmVolume;
        UpdateSEVolume(seVolume);
        UpdateBGMVolume(bgmVolume);

        Close();
    }
}
