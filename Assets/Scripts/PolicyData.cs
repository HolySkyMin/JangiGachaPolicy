using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyData : MonoBehaviour
{
	public int MoneyDeltaValue;
	public float ApprovalDeltaValue;
	public int PopulationDeltaValue;
	public float HarvestableDeltaValue;

	public void ApplyPolicy()
	{
		StageManager.Instance.Money += MoneyDeltaValue;
		StageManager.Instance.Approval += ApprovalDeltaValue;
		StageManager.Instance.Population += PopulationDeltaValue;
		StageManager.Instance.Harvestable += HarvestableDeltaValue;
	}
}