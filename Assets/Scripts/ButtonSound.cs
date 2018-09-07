using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void Play()
    {
        SoundManager.PlaySoundEffect("effect_buttontouch");
    }
}