using System;

[Serializable]
public struct SaveData
{
    public int CurrentStage;
    public long[] CollectedCards;
    public long CurrentMoney;
    public int CurrentPopulation;
    public float CurrentApproval;
    public int CurrentGutPrice;
    public int PolicyThreshold;
    public int CurrentPolicyCount;

    public SaveData(bool dummy)
    {
        CurrentStage = -1;
        CollectedCards = new long[] { 0L, 0L, 0L, 0L, 0L, 0L };
        CurrentMoney = 3000000L;
        CurrentPopulation = 50000;
        CurrentApproval = 55f;
        CurrentGutPrice = 10000;
        PolicyThreshold = 20;
        CurrentPolicyCount = 20;
    }

    public void LoadToGame()
    {
        GameManager.Instance.StageLevel = CurrentStage;
        GameManager.Instance.Money = CurrentMoney;
        GameManager.Instance.Population = CurrentPopulation;
        GameManager.Instance.Approval = CurrentApproval;
        GameManager.Instance.GutPrice = CurrentGutPrice;
        GameManager.Instance.PolicyThreshold = PolicyThreshold;
        GameManager.Instance.PolicyCount = CurrentPolicyCount;
        GameManager.Instance.Cards = CollectedCards.Clone() as long[];
    }

    public void ReadFromGame()
    {
        CurrentStage = GameManager.Instance.StageLevel - 1;
        CurrentMoney = GameManager.Instance.Money;
        CurrentPopulation = GameManager.Instance.Population;
        CurrentApproval = GameManager.Instance.Approval;
        CurrentGutPrice = GameManager.Instance.GutPrice;
        PolicyThreshold = GameManager.Instance.PolicyThreshold;
        CurrentPolicyCount = GameManager.Instance.PolicyCount;
        CollectedCards = GameManager.Instance.Cards.Clone() as long[];
    }
}