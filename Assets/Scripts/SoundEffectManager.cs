using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    int maxAuds = 100;//�ѹ��� 100���� ȿ���������� ���
    static SoundEffectManager _soundEffectManager;
    public AudioSource[] soundBar;
    public static SoundEffectManager soundEffectManager
    {
        get
        {
            if (!_soundEffectManager) //ȣ��ƴµ� SE�Ŵ����� ���� �� �����ϴ� ����
            {
                GameObject SE_Manager = new GameObject("SE_manager");
                _soundEffectManager = SE_Manager.AddComponent(typeof(SoundEffectManager)) as SoundEffectManager;
                _soundEffectManager.soundBar = new AudioSource[_soundEffectManager.maxAuds]; //�ƽ��������ŭ�� ��ü���� �����Ұ���

                for (int i = 0; i < _soundEffectManager.soundBar.Length; i++)
                {
                    GameObject go = Instantiate(Resources.Load("Prefabs/SoundBar") as GameObject);
                    go.transform.SetParent(SE_Manager.transform);
                    _soundEffectManager.soundBar[i] = go.GetComponent<AudioSource>(); //����ٵ��� ������Ʈ�� soundBar�� �Ҵ�
                }

                DontDestroyOnLoad(SE_Manager);
            }

            return _soundEffectManager;
        }
    } //�̱��� �������� �׻�����


    public static void SoundEffect(string soundName) //��ģ���� ������ ȣ���Ͽ� ����մϴ�
    {
        soundEffectManager.SoundPlay(Resources.Load<AudioClip>("Sounds/" + soundName) as AudioClip);
    }

    public static void SoundEffect(AudioClip ac)
    {
        soundEffectManager.SoundPlay(ac);
    }

    public void SoundPlay(AudioClip ac)
    {
        int playingAuds = 1; //�� �޼��尡 ����Ǹ鼭 �ּ� 1���� ������ ���̹Ƿ� 1
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
        float goVolume = (0.3f + (0.7f / playingAuds));// * SettingsManager.getSE; //�÷��̵ǰ� �ִ� ȿ������ ���� ���� ������ ������, foreach �ȿ� ������ ������ŭ �ݺ�����ϹǷ� ���� ���
        foreach (AudioSource audioSource in soundEffectManager.soundBar)
        {
            audioSource.volume = goVolume;
        }
        soundEffectManager.soundBar[notplayingAud].clip = ac;
        soundEffectManager.soundBar[notplayingAud].Play();
    }
}