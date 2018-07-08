using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolicyData : MonoBehaviour
{
	public int MoneyDeltaValue;
	public float ApprovalDeltaValue;
	public int PopulationDeltaValue;
	public float HarvestableDeltaValue;
	public Text NameText;
	public Text DescriptionText;
	public Text MoneyText, ApprovalText, PopulationText, HarvestableText;

	public void DisplayValueInfo()
	{
		if(MoneyDeltaValue >= 0L)
			MoneyText.text = "+";
		MoneyText.text += MoneyDeltaValue.ToString();
		if(ApprovalDeltaValue >= 0)
			ApprovalText.text = "+";
		ApprovalText.text += ApprovalDeltaValue.ToString("N0") + "%";
		if(PopulationDeltaValue >= 0)
			PopulationText.text = "+";
		PopulationText.text += PopulationDeltaValue.ToString();
		if(HarvestableDeltaValue >= 0)
			HarvestableText.text = "+";
		HarvestableText.text += HarvestableDeltaValue.ToString("N0") + "%";
	}

	public void ApplyPolicy()
	{
		if(StageManager.Instance.Money + MoneyDeltaValue < 0L || 
			StageManager.Instance.Population + PopulationDeltaValue < 0) {return;}
		
		StageManager.Instance.Money += (long)MoneyDeltaValue;
		StageManager.Instance.Approval *= (100 + ApprovalDeltaValue) / 100f;
		StageManager.Instance.Population += PopulationDeltaValue;
		StageManager.Instance.Harvestable *= (100 + HarvestableDeltaValue) / 100f;

		if(StageManager.Instance.Approval >= 100)
			StageManager.Instance.Approval = 100;
		if(StageManager.Instance.Harvestable >= 100)
			StageManager.Instance.Harvestable = 100;
		if(StageManager.Instance.Harvestable <= 0)
			StageManager.Instance.Harvestable = 0;

		StageManager.Instance.PolicyDimmer.SetActive(true);
		StageManager.Instance.PolicyAvailable.SetActive(false);
		Gacha.gachaCount = 0;
		StageManager.Instance.UpdatePolicyGachaCount();
	}
}