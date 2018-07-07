using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGachaResult : MonoBehaviour
{
	public Text[] Amount;

	public void UpdateResult(int[] resultArray)
	{
		for(int i = 0; i < resultArray.Length; i++)
		{
			Amount[i].text = resultArray[i].ToString();
		}
	}
}
