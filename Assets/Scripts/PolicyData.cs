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

    private void Update()
    {
        if ((GameManager.Instance.Money + MoneyDeltaValue < 0 ||
            GameManager.Instance.Population + PopulationDeltaValue < 0) && 
            gameObject.GetComponent<Button>().interactable)
            gameObject.GetComponent<Button>().interactable = false;
        if (GameManager.Instance.Money + MoneyDeltaValue >= 0 && 
            GameManager.Instance.Population + PopulationDeltaValue >= 0 && 
            !gameObject.GetComponent<Button>().interactable)
            gameObject.GetComponent<Button>().interactable = true;
    }

    public void DisplayValueInfo()
    {
        if (MoneyDeltaValue >= 0L)
            MoneyText.text = "+";
        MoneyText.text += MoneyDeltaValue.ToString("N0");
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
        GutPriceText.text += GutPriceDeltaValue.ToString("N0") + "$";
    }

    public void ApplyPolicy()
    {
        if (GameManager.Instance.Money + MoneyDeltaValue < 0L ||
            GameManager.Instance.Population + PopulationDeltaValue < 0) { return; }

        GameManager.Instance.Money += (long)MoneyDeltaValue;
        if (IsPercentPoint)
            GameManager.Instance.Approval += ApprovalDeltaValue;
        else
            GameManager.Instance.Approval *= (100 + ApprovalDeltaValue) / 100f;
        GameManager.Instance.Population += PopulationDeltaValue;
        GameManager.Instance.GutPrice += GutPriceDeltaValue;

        if (GameManager.Instance.Approval >= 100)
            GameManager.Instance.Approval = 100;
        if (GameManager.Instance.Approval < 0)
            GameManager.Instance.Approval = 0;

        GameManager.Instance.ResetPolicyCount();
        StageManager.Instance.PolicyDimmer.SetActive(true);
        StageManager.Instance.PolicyAvailable.SetActive(false);
        StageManager.Instance.UpdatePolicyGachaCount();
    }
}