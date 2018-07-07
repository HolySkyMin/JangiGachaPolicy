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
	public static int[] cards;
	public static bool isSSSREarned;

	private int extractAmount = 0;

	private void Start()
	{
		if(Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	private void Update()
	{
		ExtractAmount.text = extractAmount.ToString();
		ExpectedEarnging.text = (extractAmount * 10000).ToString() + "$";
	}

	public void DisplayCardCount()
	{
		if(cards != null)
		{
			for(int i = 0; i < cards.Length; i++)
			{
				CardsAmountText[i].text = cards[i].ToString();
			}
		}
		isSSSREarned = false;
	}

	public void ChangeExtractAmount(int changeAmount)
	{
		if(extractAmount + changeAmount >= 0 && extractAmount + changeAmount <= StageManager.Instance.Population * (StageManager.Instance.Harvestable / 100))
		{
			extractAmount += changeAmount;
		}
	}

	public void GetMoney()
	{
		StageManager.Instance.Money += extractAmount * 10000;
		StageManager.Instance.Population -= extractAmount;
		extractAmount = 0;
	}
}