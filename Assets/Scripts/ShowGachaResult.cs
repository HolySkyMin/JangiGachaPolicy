using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGachaResult : MonoBehaviour
{
    public GameObject ResultPanel, MenuDimmer;
    public Text[] Amount;

    private void Start()
    {
        ResultPanel.SetActive(false);
    }

    public void UpdateResult(int[] resultArray)
    {
        ResultPanel.SetActive(true);

        for (int i = 0; i < resultArray.Length; i++)
        {
            Amount[i].text = resultArray[i].ToString();
        }
    }

    public void AcceptClick()
    {
        ResultPanel.SetActive(false);
        MenuDimmer.SetActive(false);
    }
}