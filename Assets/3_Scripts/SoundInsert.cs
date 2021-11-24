using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInsert : MonoBehaviour
{
    public AudioClip onAwake, onDie;

    private void Awake()
    {
        if (onAwake)
        {
            SoundManager.SoundEffect(onAwake);
        }
    }
    private void OnDisable()
    {
        if (onDie)
        {
            SoundManager.SoundEffect(onDie);
        }
    }
}
