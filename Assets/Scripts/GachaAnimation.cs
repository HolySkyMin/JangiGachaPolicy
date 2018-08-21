using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaAnimation : MonoBehaviour
{
    public GameObject GachaAnimObject, GachaParticle, GachaCardPack, MenuDimmer;
    public ShowGachaResult ResultScreen;

    private int clickGoal, clickCount;
    private int[] gachaRes;
    private Color lastEffectColor;

    public void Play(int click, int[] gachaResult)
    {
        clickGoal = click;
        clickCount = 0;
        gachaRes = gachaResult.Clone() as int[];
        GachaAnimObject.SetActive(true);
        MenuDimmer.SetActive(true);
        GachaParticle.transform.localScale = Vector3.one;
        GachaParticle.transform.eulerAngles = Vector3.zero;
        GachaParticle.GetComponent<Image>().color = Color.white;

        if (gachaResult[5] > 0)
            lastEffectColor = new Color32(255, 128, 0, 255);
        else if (gachaResult[4] > 0)
            lastEffectColor = Color.blue;
        else if (gachaResult[3] > 0)
            lastEffectColor = Color.magenta;
        else if (gachaResult[2] > 0)
            lastEffectColor = Color.yellow;
        else if (gachaResult[1] > 0)
            lastEffectColor = new Color32(180, 152, 90, 255);
        else
            lastEffectColor = Color.white;
    }

    private void Update()
    {
        if(GachaAnimObject.activeSelf)
        {
            if(GachaParticle.transform.localScale.x > 1)
            {
                GachaParticle.transform.localScale -= new Vector3((1 + 0.2f * clickCount) * Time.deltaTime, (1 + 0.2f * clickCount) * Time.deltaTime, 0);
                if (GachaParticle.transform.localScale.x < 1)
                    GachaParticle.transform.localScale = Vector3.one;
            }
            if(GachaCardPack.transform.localScale.x > 1)
            {
                GachaCardPack.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
                if (GachaCardPack.transform.localScale.x < 1)
                    GachaCardPack.transform.localScale = Vector3.one;
            }
            if(clickCount < clickGoal && Input.anyKeyDown)
            {
                GachaParticle.transform.localScale = new Vector3(2 + 0.2f * clickCount, 2 + 0.2f * clickCount, 1);
                GachaParticle.transform.eulerAngles = new Vector3(0, 0, 30 * clickCount);
                GachaCardPack.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                clickCount++;
            }

            if (clickCount == clickGoal)
            {
                if (GachaParticle.GetComponent<Image>().color != lastEffectColor)
                    GachaParticle.GetComponent<Image>().color = lastEffectColor;
                if(GachaParticle.transform.localScale == Vector3.one)
                {
                    GachaAnimObject.SetActive(false);
                    ResultScreen.UpdateResult(gachaRes);
                }
            }
        }
    }
}