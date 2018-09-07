using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentSaveSlot { get; private set; }
    public bool GotSSSR { get; set; }
    public GameState State { get; set; }
    public StageState Phase { get; set; }

    #region 핵심 게임 데이터
    public int StageLevel;
    public long Money;
    public int Population;
    public float Approval;
    public int GutPrice;
    public int PolicyThreshold;
    public int PolicyCount;
    public long[] Cards;
    #endregion

    private SaveData CurrentGameData;
    private List<string> StageName = new List<string>();
    private List<PolicyDataCore> PositivePolicy = new List<PolicyDataCore>();
    private List<PolicyDataCore> ModeratePolicy = new List<PolicyDataCore>();
    private List<PolicyDataCore> NegativePolicy = new List<PolicyDataCore>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (State == GameState.Title)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }

    #region 게임 데이터 저장, 삭제 및 불러오기
    public void CreateNewGame(int slot)
    {
        CurrentGameData = new SaveData(true);
        CurrentSaveSlot = slot;
        CurrentGameData.LoadToGame();

        SceneChanger.LoadScene("Intro Scene");
    }

    public void LoadSavedGame(int slot)
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + slot.ToString()))
            return;
        FileStream stream = File.Open(Application.persistentDataPath + "/save" + slot.ToString(), FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        CurrentGameData = (SaveData)formatter.Deserialize(stream);
        stream.Close();
        CurrentSaveSlot = slot;
        CurrentGameData.LoadToGame();

        SceneChanger.LoadScene("Gacha Scene");
    }

    public void SaveCurrentGame()
    {
        CurrentGameData.ReadFromGame();
        FileStream stream = File.Open(Application.persistentDataPath + "/save" + CurrentSaveSlot.ToString(), FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, CurrentGameData);
        stream.Close();
    }

    public void DeleteSavedGame(int slot)
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + slot.ToString()))
            return;
        File.Delete(Application.persistentDataPath + "/save" + slot.ToString());
    }
    #endregion

    #region 스테이지 및 정책 불러오기
    public void ReadStageList()
    {
        TextAsset data = Resources.Load("Data/stagelist") as TextAsset;
        string[] arr = Regex.Split(data.text, @"\r\n|\n\r|\n|\r");

        for (int i = 1; i < arr.Length; i++)
        {
            string[] row = arr[i].Split(',');
            if (row.Length == 2)
            {
                StageName.Add(row[1]);
            }
        }
    }

    public void ReadPolicyList()
    {
        TextAsset data = Resources.Load("Data/policyList") as TextAsset;
        string[] arr = Regex.Split(data.text, @"\r\n|\n\r|\n|\r");

        for (int i = 1; i < arr.Length; i++)
        {
            string[] row = arr[i].Split(',');
            if (row.Length == 8)
            {
                PolicyDataCore policy;
                policy.Name = row[0];
                policy.Description = row[1];
                policy.MoneyDeltaValue = int.Parse(row[2]);
                policy.ApprovalDeltaValue = float.Parse(row[3]);
                policy.PopulationDeltaValue = int.Parse(row[4]);
                policy.GutPriceDeltaValue = int.Parse(row[5]);

                int isPP = int.Parse(row[7]);
                if (isPP == 0)
                    policy.IsPercentPoint = false;
                else
                    policy.IsPercentPoint = true;

                int positivity = int.Parse(row[6]);
                if (positivity == 1)
                    NegativePolicy.Add(policy);
                else if (positivity == 2)
                    ModeratePolicy.Add(policy);
                else if (positivity == 3)
                    PositivePolicy.Add(policy);
            }
        }
    }
    #endregion

    public string UpdateAndGetStage()
    {
        StageLevel++;
        if (StageLevel < 30)
            return StageName[StageLevel];
        else
            return "통상 가챠 시즌이 시작되었습니다!";
    }

    #region 적출 관련 메소드
    public int CalculateMoney(int dieCount)
    {
        return dieCount * GutPrice;
    }

    public void MakeMoney(int dieCount)
    {
        Money += CalculateMoney(dieCount);
        Population -= dieCount;
        Approval -= dieCount / 500f;
        if (Approval < 0)
            Approval = 0;
    }
    #endregion

    #region 가챠 관련 메소드
    public int[] ExecuteGacha(int count)
    {
        Money -= count * 3;
        int[] card = new int[6];
        for(int i = 0; i < count; i++)
        {
            int pick = UnityEngine.Random.Range(1, 10000001);
            if (pick > 2500000)
                card[0]++;
            else if (pick > 1000000)
                card[1]++;
            else if (pick > 100201)
                card[2]++;
            else if (pick > 201)
                card[3]++;
            else if (pick > 1)
                card[4]++;
            else
                card[5]++;
        }
        for (int i = 0; i < 6; i++)
            Cards[i] += card[i];
        if (card[5] > 0)
            GotSSSR = true;
        return card;
    }
    #endregion

    #region 정책 관련 메소드
    public List<PolicyDataCore> MakePolicyList()
    {
        PolicyDataCore[] policy = new PolicyDataCore[0];
        int rand = UnityEngine.Random.Range(1, 4);
        if (rand == 1)
            policy = NegativePolicy.ToArray();
        else if (rand == 2)
            policy = ModeratePolicy.ToArray();
        else if (rand == 3)
            policy = PositivePolicy.ToArray();

        bool[] checksum = new bool[policy.Length];
        List<PolicyDataCore> finalList = new List<PolicyDataCore>();
        for(int i = 0; i < 3; i++)
        {
            int index;
            do
                index = UnityEngine.Random.Range(0, policy.Length);
            while (checksum[index] == true);
            finalList.Add(policy[index]);
            checksum[index] = true;
        }
        return finalList;
    }

    public void SkipPolicy()
    {
        Approval *= 8.5f / 10f;
        ResetPolicyCount();
    }

    public void ResetPolicyCount()
    {
        PolicyCount = 0;
        PolicyThreshold = 20 + (StageLevel / 5 * 15);
    }
    #endregion

    public bool CheckGameOver()
    {
        if (Approval < 4f)
            return true;
        if (Money < 3 && Population < 10 && PolicyCount < PolicyThreshold)
            return true;
        else
            return false;
    }
}