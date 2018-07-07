using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
	public static StageManager Instance;

	public int StageIndex, Money, Population, Harvestable;
	public List<int> StageNumber;
    public List<string> StageName;
	public Text StageNumberText, StageNameText, MoneyText, PopulationText, HarvestableText;
	public GameObject EarnedPanel;

	public delegate void SpecialEvent();

	void Start () 
	{
		if(Instance == null)
			Instance = this;
		else
			Destroy(this);

		ReadStageList();
	}

	void Update () 
	{
		MoneyText.text = Money.ToString();
		PopulationText.text = Population.ToString();
		HarvestableText.text = Harvestable.ToString();
	}

	private void ReadStageList()
	{
		TextAsset data = Resources.Load("stageList") as TextAsset;
		string[] arr = Regex.Split(data.text, @"\r\n|\n\r|\n|\r");
		// Assume that there are only two keys.
		for(int i = 1; i < arr.Length; i++)
		{
			string[] row = arr[i].Split(',');
			int idx = int.Parse(row[0]);
			StageNumber.Add(idx);
			StageName.Add(row[1]);
		}
	}

	public void UpdateStageIndex()
	{
		StageIndex++;
		StageNumberText.text = StageNumber[StageIndex].ToString();
		StageNameText.text = StageName[StageIndex].ToString();
	}
}
