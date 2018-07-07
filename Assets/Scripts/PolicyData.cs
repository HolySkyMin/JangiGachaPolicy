using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolicyData : MonoBehaviour
{
	public string Name;
	public string Description;
	public int MoneyDeltaValue;
	public float ApprovalDeltaValue;
	public int PopulationDeltaValue;
	public float HarvestableDeltaValue;
	public Text NameText;
	public Text DescriptionText;

	public void ApplyPolicy()
	{
		StageManager.Instance.Money += MoneyDeltaValue;
		StageManager.Instance.Approval += ApprovalDeltaValue;
		StageManager.Instance.Population += PopulationDeltaValue;
		StageManager.Instance.Harvestable += HarvestableDeltaValue;
	}
}