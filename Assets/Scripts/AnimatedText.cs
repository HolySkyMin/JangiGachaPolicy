using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedText : MonoBehaviour
{
    public Text Body;
    public bool PlayCompleted = true;
    private string myText;

    public void Play(string text)
    {
        myText = text;
        PlayCompleted = false;
        StartCoroutine(TextAnimation(text));
    }

    public void Stop()
    {
        StopAllCoroutines();
        PlayCompleted = true;
        Body.text = myText;
    }

    IEnumerator TextAnimation(string text)
    {
        int length = text.Length;

        for(int i = 1; i < length; i++)
        {
            Body.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
        PlayCompleted = true;
        Body.text = text;
    }
}