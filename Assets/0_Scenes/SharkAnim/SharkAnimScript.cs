using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAnimScript : MonoBehaviour
{
    public GameObject wave;
    public AudioClip waveEffect;
    public void Wave()
    {
        Instantiate(wave, transform.position, Quaternion.identity);
    }
    public void SoundEffect()
    {
        SoundManager.SoundEffect(waveEffect);
    }
}
