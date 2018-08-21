using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public static Status Instance;
    public Text[] CardsAmountText;
    public Text ExtractAmount;
    public Text ExpectedEarnging;
    public static long[] cards;
    public static bool isSSSREarned;

    public int gutPrice;
    private int extractAmount = 0;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        ExtractAmount.text = extractAmount.ToString();
        ExpectedEarnging.text = (extractAmount * gutPrice).ToString() + "$";
    }

    public void DisplayCardCount()
    {
        if (cards != null)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                CardsAmountText[i].text = cards[i].ToString();
            }
        }
        isSSSREarned = false;
    }

    public void ChangeExtractAmount(int changeAmount)
    {
        if (extractAmount + changeAmount >= 0 && extractAmount + changeAmount <= StageManager.Instance.Population)
        {
            extractAmount += changeAmount;
        }
    }

    public void GetMoney()
    {
        //float absolutePeople = StageManager.Instance.Population * (StageManager.Instance.Harvestable / 100);
        StageManager.Instance.Money += extractAmount * gutPrice;
        StageManager.Instance.Population -= extractAmount;
        StageManager.Instance.Approval -= extractAmount / 500f;
        //absolutePeople -= extractAmount;
        //StageManager.Instance.Harvestable = absolutePeople / StageManager.Instance.Population * 100;
        extractAmount = 0;
    }
}