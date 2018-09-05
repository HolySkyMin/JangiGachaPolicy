using System;
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

        yield return ShowText("먼 미래, 세상이 기계로 뒤덮인 날..." + Environment.NewLine + Environment.NewLine + "당신은 어떻게든 네오-아시아 대통령의 자리에 오르게 된다.");
        //yield return ShowText("당신은 어떻게든 네오-아시아 대통령의 자리에 오르게 된다.");
        yield return ShowText("매일 고된 일과 암투에 시달리던 당신에게 나타난 한 모바일 게임!");
        yield return ShowText("무지막지한 인기와 매출로 네오-아시아를 휩쓸고 있는 '도와줘! 아이돌과 함께하는 선샤인 라이브 투 스타라이트!'!!!");
        yield return ShowText("반짝반짝 빛나는 일러스트, 눈 돌아갈 정도의 게임 내 성능, 그리고 무엇보다도 너무너무 귀여운 아이돌들!");
        yield return ShowText("이 모든 것을 쟁취하기 위하여, 이제 당신은 전설로만 전해지는 기간 한정 SSSSR 카드를 획득하기 위한 여정을 떠난다!");
        yield return ShowText("물론, 그 과정에서 벌어질 수많은 인간들의 희생과 저항은 순전히 당신의 몫이다.");
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