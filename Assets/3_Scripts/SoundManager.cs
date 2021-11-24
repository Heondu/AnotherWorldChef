using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    int maxAuds = 100;
    static SoundManager _soundManager;
    public AudioSource[] soundBar;
    public AudioSource soundBarBGM;
    private float seVolume = 1;
    private float bgmVolume = 1;
    public static SoundManager soundManager
    {
        get
        {
            if (!_soundManager)
            {
                GameObject SE_Manager = new GameObject("SE_manager");
                _soundManager = SE_Manager.AddComponent(typeof(SoundManager)) as SoundManager;
                _soundManager.soundBar = new AudioSource[_soundManager.maxAuds];

                for (int i = 0; i < _soundManager.soundBar.Length; i++)
                {
                    GameObject go = new GameObject("SoundBar");
                    _soundManager.soundBar[i] = go.AddComponent<AudioSource>();
                    go.transform.SetParent(SE_Manager.transform);
                }
                GameObject clone = new GameObject("SoundBarBGM");
                _soundManager.soundBarBGM = clone.AddComponent<AudioSource>();
                _soundManager.soundBarBGM.loop = true;
                clone.transform.SetParent(SE_Manager.transform);

                DontDestroyOnLoad(SE_Manager);
                SceneManager.sceneLoaded += _soundManager.SoundAllMute;
                SceneManager.sceneLoaded += _soundManager.StopBGM;
            }

            return _soundManager;
        }
    }

    public static void SoundEffect(AudioClip ac)
    {
        soundManager.SoundPlay(ac);
    }

    public void SoundPlay(AudioClip ac)
    {
        int playingAuds = 1;
        int notplayingAud = 0;
        for (int i = 0; i < soundManager.soundBar.Length; i++)
        {
            if (soundManager.soundBar[i].isPlaying)
            {
                playingAuds++;
            }
            else
            {
                notplayingAud = i;
            }
        }
        float goVolume = (0.3f + (0.7f / playingAuds)) * seVolume;
        foreach (AudioSource audioSource in soundManager.soundBar)
        {
            audioSource.volume = goVolume;
        }
        soundManager.soundBar[notplayingAud].clip = ac;
        soundManager.soundBar[notplayingAud].Play();
    }

    private void SoundAllMute(Scene scene, LoadSceneMode loadSceneMode)
    {
        foreach (AudioSource audioSource in soundManager.soundBar)
        {
            audioSource.Stop();
        }
    }

    public static void PlayBGM(AudioClip ac)
    {
        soundManager.soundBarBGM.clip = ac;
        soundManager.soundBarBGM.Play();
    }

    private void StopBGM(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "LoadingScene")
        {
            soundManager.soundBarBGM.Pause();
        }
        else
        {
            soundManager.soundBarBGM.UnPause();
        }
    }

    public void SetSEVolume(float value)
    {
        seVolume = value;
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;

        soundManager.soundBarBGM.volume = bgmVolume;
    }

    public float GetSEVolume()
    {
        return seVolume;
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }
}