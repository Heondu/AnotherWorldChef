using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    int maxAuds = 100;//한번에 100개의 효과음까지만 재생
    static SoundEffectManager _soundEffectManager;
    public AudioSource[] soundBar;
    public static SoundEffectManager soundEffectManager
    {
        get
        {
            if (!_soundEffectManager) //호출됐는데 SE매니저가 없을 시 생성하는 과정
            {
                GameObject SE_Manager = new GameObject("SE_manager");
                _soundEffectManager = SE_Manager.AddComponent(typeof(SoundEffectManager)) as SoundEffectManager;
                _soundEffectManager.soundBar = new AudioSource[_soundEffectManager.maxAuds]; //맥스오디오만큼의 객체들을 생성할것임

                for (int i = 0; i < _soundEffectManager.soundBar.Length; i++)
                {
                    GameObject go = Instantiate(Resources.Load("Prefabs/SoundBar") as GameObject);
                    go.transform.SetParent(SE_Manager.transform);
                    _soundEffectManager.soundBar[i] = go.GetComponent<AudioSource>(); //사운드바들의 컴포넌트를 soundBar에 할당
                }

                DontDestroyOnLoad(SE_Manager);
            }

            return _soundEffectManager;
        }
    } //싱글톤 형식으로 항상유지


    public static void SoundEffect(string soundName) //이친구를 실제로 호출하여 사용합니다
    {
        soundEffectManager.SoundPlay(Resources.Load<AudioClip>("Sounds/" + soundName) as AudioClip);
    }

    public static void SoundEffect(AudioClip ac)
    {
        soundEffectManager.SoundPlay(ac);
    }

    public void SoundPlay(AudioClip ac)
    {
        int playingAuds = 1; //이 메서드가 실행되면서 최소 1개는 생성될 것이므로 1
        int notplayingAud = 0;
        for (int i = 0; i < soundEffectManager.soundBar.Length; i++)
        {
            if (soundEffectManager.soundBar[i].isPlaying)
            {
                playingAuds++;
            }
            else
            {
                notplayingAud = i;
            }
        }
        float goVolume = (0.3f + (0.7f / playingAuds));// * SettingsManager.getSE; //플레이되고 있는 효과음의 수에 따라서 음량을 조절함, foreach 안에 넣으면 갯수만큼 반복계산하므로 먼저 계산
        foreach (AudioSource audioSource in soundEffectManager.soundBar)
        {
            audioSource.volume = goVolume;
        }
        soundEffectManager.soundBar[notplayingAud].clip = ac;
        soundEffectManager.soundBar[notplayingAud].Play();
    }
}