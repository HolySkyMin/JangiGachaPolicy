using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
	public static Status Instance;
	public Text[] CardsAmountText;
	public Text ExtractAmount;
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
		if(extractAmount + changeAmount >= 0)
		{
			extractAmount += changeAmount;
		}

		ExtractAmount.text = extractAmount.ToString();
	}
}