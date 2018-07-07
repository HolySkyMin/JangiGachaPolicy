using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
	public static StageManager Instance;
	public static GameState State = GameState.General;

	public int StageIndex = 0, Population;
	public long Money;
	public float Approval, Harvestable;
	public List<int> StageNumber;
    public List<string> StageName;
	public Text StageNumberText, StageNameText, MoneyText, ApprovalText, PopulationText, HarvestableText, PolicyGachaText;
	public GameObject ResultPanel, EarnedPanel, EarnedPanelBtn, PolicyTemplate, PolicyDimmer, NewStagePopup;
	public RectTransform MenuPanel, GameOverPanel, PolicyListBody;
	public GameObject[] MenuEffect = new GameObject[4];
	public RectTransform[] GamePanel = new RectTransform[4];

	private List<GameObject> PolicyDisplayList = new List<GameObject>();
	private List<PolicyDataCore> PositivePolicy = new List<PolicyDataCore>();
	private List<PolicyDataCore> ModeratePolicy = new List<PolicyDataCore>();
	private List<PolicyDataCore> NegativePolicy = new List<PolicyDataCore>();

	void Start () 
	{
		if(Instance == null)
			Instance = this;
		else
			Destroy(this);

		ReadStageList();
		ReadPolicyList();
		
		Initialize();
	}

	void Update () 
	{
		MoneyText.text = Money.ToString();
		ApprovalText.text = Approval.ToString("N2");
		PopulationText.text = Population.ToString();
		HarvestableText.text = Harvestable.ToString("N2");

		if(State == GameState.General && Approval <= 0)
			GameOver();
		if(State == GameState.General && Money < 3 && Population * (Harvestable / 100) < 10f && Gacha.gachaCount < 20)
			GameOver();
	}

	public void Initialize()
	{
		Money = 3000000L;
		Approval = 45;
		Population = 5000000;
		Harvestable = 25;
		StageIndex = -1;
		Gacha.gachaCount = 20;
		Status.cards = new long[6] {0L, 0L, 0L, 0L, 0L, 0L};
		Status.Instance.DisplayCardCount();

		UpdateStage();
		ResetGameState();
		UpdatePolicyGachaCount();
	}

	private void ReadStageList()
	{
		TextAsset data = Resources.Load("Data/stagelist") as TextAsset;
		string[] arr = Regex.Split(data.text, @"\r\n|\n\r|\n|\r");

		for(int i = 1; i < arr.Length; i++)
		{
			string[] row = arr[i].Split(',');
			if(row.Length == 2)
			{
				int idx = int.Parse(row[0]);
				StageNumber.Add(idx);
				StageName.Add(row[1]);
			}
		}

		StageNameText.text = StageName[StageIndex].ToString();
	}

	private void ReadPolicyList()
	{
		TextAsset data = Resources.Load("Data/policyList") as TextAsset;
		string[] arr = Regex.Split(data.text, @"\r\n|\n\r|\n|\r");

		for(int i = 1; i < arr.Length; i++)
		{
			string[] row = arr[i].Split(',');
			if(row.Length == 7)
			{
				PolicyDataCore policy;
				policy.Name = row[0];
				policy.Description = row[1];
				policy.MoneyDeltaValue = int.Parse(row[2]);
				policy.ApprovalDeltaValue = float.Parse(row[3]);
				policy.PopulationDeltaValue = int.Parse(row[4]);
				policy.HarvestableDeltaValue = float.Parse(row[5]);

				int positivity = int.Parse(row[6]);
				if(positivity == 1)
					NegativePolicy.Add(policy);
				else if(positivity == 2)
					ModeratePolicy.Add(policy);
				else if(positivity == 3)
					PositivePolicy.Add(policy);
			}
		}
	}

	public void ChangePanel(int index)
	{
		GamePanel[index].SetAsLastSibling();
		MenuPanel.SetAsLastSibling();
		GameOverPanel.SetAsLastSibling();
		for(int i = 0; i < MenuEffect.Length; i++)
		{
			if(i == index)
				MenuEffect[i].SetActive(true);
			else
				MenuEffect[i].SetActive(false);
		}
	}

	public void CheckSSSSREarned()
	{
		if(Status.isSSSREarned)
		{
			EarnedPanel.SetActive(true);
			Status.isSSSREarned = false;
			StartCoroutine(SSSSRButtonAnim());
		}
		else
			State = GameState.General;
	}

	IEnumerator SSSSRButtonAnim()
	{
		yield return new WaitForSeconds(2);
		EarnedPanelBtn.SetActive(true);
	}

	public void UpdatePolicy()
	{
		for(int i = 0; i < PolicyDisplayList.Count; i++)
			Destroy(PolicyDisplayList[i]);
		PolicyDisplayList.Clear();

		PolicyDataCore[] policy = new PolicyDataCore[0];
		int rand = Random.Range(1, 4); // 1, 2, 3 can be picked.
		if(rand == 1)
			policy = NegativePolicy.ToArray();
		else if(rand == 2)
			policy = ModeratePolicy.ToArray();
		else if(rand == 3)
			policy = PositivePolicy.ToArray();
		
		bool[] checksum = new bool[policy.Length];
		for(int i = 0; i < 3; i++)
		{
			int index;
			do
			{
				index = Random.Range(0, policy.Length); // 0, 1, ..., policy.Length - 1 can be picked.
			}
			while(checksum[index] == true);
			checksum[index] = true;
			GameObject newObj = Instantiate(PolicyTemplate);
			PolicyData newPolicy = newObj.GetComponent<PolicyData>();
			newPolicy.NameText.text = policy[index].Name;
			newPolicy.DescriptionText.text = policy[index].Description;
			newPolicy.MoneyDeltaValue = policy[index].MoneyDeltaValue;
			newPolicy.ApprovalDeltaValue = policy[index].ApprovalDeltaValue;
			newPolicy.PopulationDeltaValue = policy[index].PopulationDeltaValue;
			newPolicy.HarvestableDeltaValue = policy[index].HarvestableDeltaValue;
			newObj.GetComponent<RectTransform>().SetParent(PolicyListBody);
			newObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			newObj.SetActive(true);
			newPolicy.DisplayValueInfo();
			PolicyDisplayList.Add(newObj);
		}
	}

	public void SkipPolicy()
	{
		Approval *= 9f / 10f;
		PolicyDimmer.SetActive(true);
		Gacha.gachaCount = 0;
		UpdatePolicyGachaCount();
	}

	public void UpdatePolicyGachaCount()
	{
		PolicyGachaText.text = ((20 + StageIndex * 5) - Gacha.gachaCount).ToString();

		if(Gacha.gachaCount >= 20 + StageIndex * 5)
		{
			PolicyDimmer.SetActive(false);
			UpdatePolicy();
		}
	}

	public void UpdateStage()
	{
		StageIndex++;
		// StageNumberText.text = StageNumber[StageIndex].ToString();
		StageNameText.text = StageName[StageIndex].ToString();

		ChangePanel(0);
		NewStagePopup.SetActive(true);
	}

	public void ResetGameState() { State = GameState.General; }

	public void GameOver()
	{
		State = GameState.GameOver;
		GameOverPanel.gameObject.SetActive(true);
	}
}
