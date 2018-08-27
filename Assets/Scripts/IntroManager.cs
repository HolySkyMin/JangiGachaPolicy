using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public AnimatedText IntroText;
    public GameObject NextText;

    private void Start()
    {
        GameManager.Instance.State = GameState.Intro;
        SoundManager.PlayBGM("bgm_intro");
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(0.34f);

        yield return ShowText("먼 미래, 세상이 기계로 뒤덮인 날...");
        yield return ShowText("당신은 어떻게든 네오-아시아 대통령의 자리에 오르게 된다.");
        yield return ShowText("매일 고된 일과 암투에 시달리던 당신에게, 네오-아시아를 휩쓴 전설적인 그 게임이 나타난다.");
        SceneChanger.LoadScene("Gacha Scene");
    }

    IEnumerator ShowText(string text)
    {
        NextText.SetActive(false);
        IntroText.Play(text);
        yield return new WaitUntil(() => IntroText.PlayCompleted == true);
        NextText.SetActive(true);
        yield return null;
        yield return new WaitUntil(() => Input.anyKeyDown == true);
    }

    private void Update()
    {
        if (!IntroText.PlayCompleted && Input.anyKeyDown)
            IntroText.Stop();
    }

    public void SkipIntro()
    {
        if (!IntroText.PlayCompleted)
            IntroText.Stop();
        SceneChanger.LoadScene("Gacha Scene");
    }
}