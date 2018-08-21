using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolicyData : MonoBehaviour
{
    public int MoneyDeltaValue;
    public float ApprovalDeltaValue;
    public int PopulationDeltaValue;
    public int GutPriceDeltaValue;
    public bool IsPercentPoint;
    public Text NameText;
    public Text DescriptionText;
    public Text MoneyText, ApprovalText, PopulationText, GutPriceText;

    public void DisplayValueInfo()
    {
        if (MoneyDeltaValue >= 0L)
            MoneyText.text = "+";
        MoneyText.text += MoneyDeltaValue.ToString();
        if (ApprovalDeltaValue >= 0)
            ApprovalText.text = "+";
        ApprovalText.text += ApprovalDeltaValue.ToString("N0") + "%";
        if (IsPercentPoint)
            ApprovalText.text += "p";
        if (PopulationDeltaValue >= 0)
            PopulationText.text = "+";
        PopulationText.text += PopulationDeltaValue.ToString();
        if (GutPriceDeltaValue >= 0)
            GutPriceText.text = "+";
        GutPriceText.text += GutPriceDeltaValue.ToString() + "$";
    }

    public void ApplyPolicy()
    {
        if (StageManager.Instance.Money + MoneyDeltaValue < 0L ||
            StageManager.Instance.Population + PopulationDeltaValue < 0) { return; }

        StageManager.Instance.Money += (long)MoneyDeltaValue;
        if (IsPercentPoint)
            StageManager.Instance.Approval += ApprovalDeltaValue;
        else
            StageManager.Instance.Approval *= (100 + ApprovalDeltaValue) / 100f;
        StageManager.Instance.Population += PopulationDeltaValue;
        Status.Instance.gutPrice += GutPriceDeltaValue;

        if (StageManager.Instance.Approval >= 100)
            StageManager.Instance.Approval = 100;

        StageManager.Instance.PolicyDimmer.SetActive(true);
        StageManager.Instance.PolicyAvailable.SetActive(false);
        Gacha.gachaCount = 0;
        StageManager.Instance.UpdatePolicyGachaCount();
    }
}