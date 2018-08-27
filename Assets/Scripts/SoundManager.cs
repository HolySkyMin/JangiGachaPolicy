using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource BGM, Effect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public static void PlayBGM(string soundName)
    {
        Instance.BGM.clip = Resources.Load("Sounds/" + soundName) as AudioClip;
        Instance.BGM.Play();
    }

    public static void PlaySoundEffect(string soundName)
    {
        Instance.Effect.PlayOneShot(Resources.Load("Sounds/" + soundName) as AudioClip);
    }
}