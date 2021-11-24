using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
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
        originSeVolume = seVolume;
        originBgmVolume = bgmVolume;
    }

    public void SetSEVolume(float value)
    {
        seVolume = Mathf.Clamp(seVolume + value, 0, 1);
        SoundManager.soundManager.SetSEVolume(seVolume);
        seFill.fillAmount = seVolume;
        seText.text = $"{Mathf.RoundToInt(seVolume * 100)} / 100";
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp(bgmVolume + value, 0, 1);
        SoundManager.soundManager.SetBGMVolume(bgmVolume);
        bgmFill.fillAmount = bgmVolume;
        bgmText.text = $"{Mathf.RoundToInt(bgmVolume * 100)} / 100";
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
        GameManager.Instance.IsStop = true;
        gameObject.SetActive(true);
    }

    private void Close()
    {
        Time.timeScale = 1;
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
        SoundManager.soundManager.SetSEVolume(originSeVolume);
        seFill.fillAmount = originSeVolume;
        seText.text = $"{Mathf.RoundToInt(originSeVolume * 100)} / 100";
        bgmVolume = originBgmVolume;
        SoundManager.soundManager.SetBGMVolume(originBgmVolume);
        bgmFill.fillAmount = originBgmVolume;
        bgmText.text = $"{Mathf.RoundToInt(originBgmVolume * 100)} / 100";

        Close();
    }
}
