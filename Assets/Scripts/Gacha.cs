using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
	public enum Rarity
	{
		N, R, SR, SSR, SSSR, SSSSR
	}

	public static int gachaCount = 0;

	public GameObject NoMoneyAlert;

	Rarity Dancha()
	{
		int randomNumber = Random.Range(1, 10000000);
		//Debug.Log(randomNumber);

		if(randomNumber > 2500000)
		{
			return Rarity.N;
		}
		else if(randomNumber > 1000000)
		{
			return Rarity.R;
		}
		else if(randomNumber > 100201)
		{
			return Rarity.SR;
		}
		else if(randomNumber > 201)
		{
			return Rarity.SSR;
		}
		else if(randomNumber > 1)
		{
			return Rarity.SSSR;
		}
		else
		{
			return Rarity.SSSSR;			
		}
	}

	public void PlayGacha(int count)
	{
		if(StageManager.Instance.Money < count * 3)
		{
			NoMoneyAlert.SetActive(true);
			return;
		}

		StageManager.State = GameState.Gacha;
		int[] GachaResult = new int[6] {0, 0, 0, 0, 0, 0};

		for (int i = 0; i < count; i++)
		{
			int a = (int)Dancha();

			GachaResult[a] = GachaResult[a] + 1;
		}
		StageManager.Instance.Money -= count * 3;

		// Debug.Log(GachaResult[0] + ", "
		// 		+ GachaResult[1] + ", "
		// 		+ GachaResult[2] + ", "
		// 		+ GachaResult[3] + ", "
		// 		+ GachaResult[4] + ", "
		// 		+ GachaResult[5]);

		gameObject.GetComponentInParent<ShowGachaResult>().UpdateResult(GachaResult);	//Show Result of Gacha
		
		if(Status.cards == null)
		{
			Status.cards = new long[6] {0, 0, 0, 0, 0, 0};
		}
		for (int i = 0; i < GachaResult.Length; i++)
		{
			Status.cards[i] += (long)GachaResult[i];
		}
		Status.Instance.DisplayCardCount();
		if(GachaResult[(int)Rarity.SSSSR] > 0)
		{
			Status.isSSSREarned = true;
		}

		gachaCount++;
		StageManager.Instance.UpdatePolicyGachaCount();
	}
}
