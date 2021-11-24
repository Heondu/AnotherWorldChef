using UnityEngine;

public class BGMChanger : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    private void Start()
    {
        SoundManager.PlayBGM(bgm);        
    }

}
