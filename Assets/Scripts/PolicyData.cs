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
		if(MoneyDeltaValue >= 0)
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
		StageManager.Instance.Money += MoneyDeltaValue;
		StageManager.Instance.Approval += ApprovalDeltaValue;
		StageManager.Instance.Population += PopulationDeltaValue;
		StageManager.Instance.Harvestable += HarvestableDeltaValue;

		if(StageManager.Instance.Approval >= 100)
			StageManager.Instance.Approval = 100;
		if(StageManager.Instance.Harvestable >= 100)
			StageManager.Instance.Harvestable = 100;

		StageManager.Instance.PolicyDimmer.SetActive(true);
		Gacha.gachaCount = 0;
		StageManager.Instance.UpdatePolicyGachaCount();
	}
}