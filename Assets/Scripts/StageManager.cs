using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public Text 
        StageNumberText, StageNameText, ExtractCountText, ExpectedMoneyText,
        MoneyText, ApprovalText, PopulationText, GutPriceText, 
        PolicyGachaText;
    public Text[] CardsText;
    public GameObject ResultPanel, EarnedPanel, EarnedPanelBtn, PolicyTemplate, PolicyAvailable, PolicyDimmer, NewStagePopup, EndingPopup, MenuDimmer;
    public RectTransform MenuPanel, GameOverPanel, PolicyListBody;
    public GachaAnimation GachaAnim;
    public GameObject[] MenuEffect = new GameObject[4];
    public RectTransform[] GamePanel = new RectTransform[4];

    private int extractCount = 0;
    private List<GameObject> PolicyDisplayList = new List<GameObject>();

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        GameManager.Instance.State = GameState.Stage;
        GameManager.Instance.Phase = StageState.General;
        GameManager.Instance.ReadStageList();
        GameManager.Instance.ReadPolicyList();

        UpdateStage();
        UpdatePolicyGachaCount();
        DisplayCardCount();
    }

    void Update()
    {
        MoneyText.text = GameManager.Instance.Money.ToString("N0");
        ApprovalText.text = GameManager.Instance.Approval.ToString("N2");
        PopulationText.text = GameManager.Instance.Population.ToString("N0");
        GutPriceText.text = GameManager.Instance.GutPrice.ToString("N0") + "$";

        if (GameManager.Instance.Phase == StageState.General && GameManager.Instance.CheckGameOver() == true)
            GameOver();
    }

    #region 기본 UI 관련 메소드
    public void ChangePanel(int index)
    {
        GamePanel[index].SetAsLastSibling();
        MenuPanel.SetAsLastSibling();
        GameOverPanel.SetAsLastSibling();
        EndingPopup.GetComponent<RectTransform>().SetAsLastSibling();
        for (int i = 0; i < MenuEffect.Length; i++)
        {
            if (i == index)
                MenuEffect[i].SetActive(true);
            else
                MenuEffect[i].SetActive(false);
        }
    }

    public void ToggleMenuDimmer(bool flag)
    {
        MenuDimmer.SetActive(flag);
    }
    #endregion

    #region 적출 관련 메소드
    public void ChangeExtractCount(int delta)
    {
        if (extractCount + delta >= 0 && extractCount + delta <= GameManager.Instance.Population)
            extractCount += delta;
        ExtractCountText.text = extractCount.ToString();
        ExpectedMoneyText.text = GameManager.Instance.CalculateMoney(extractCount).ToString("N0") + "$";
    }

    public void Extract()
    {
        GameManager.Instance.MakeMoney(extractCount);
        extractCount = 0;
        ExtractCountText.text = extractCount.ToString();
        ExpectedMoneyText.text = GameManager.Instance.CalculateMoney(extractCount).ToString();
    }
    #endregion

    #region 가챠 관련 메소드
    public void DoGacha(int count)
    {
        GameManager.Instance.Phase = StageState.Gacha;
        ToggleMenuDimmer(true);
        int[] gachaData = GameManager.Instance.ExecuteGacha(count);
        GachaAnim.Play((int)Mathf.Log10(count) + 1, gachaData);

        DisplayCardCount();
        GameManager.Instance.PolicyCount++;
        UpdatePolicyGachaCount();
    }

    private void DisplayCardCount()
    {
        for (int i = 0; i < 6; i++)
            CardsText[i].text = GameManager.Instance.Cards[i].ToString("N0");
    }

    public void CheckSSSSREarned()
    {
        if (GameManager.Instance.GotSSSR)
        {
            EarnedPanel.SetActive(true);
            GameManager.Instance.GotSSSR = false;
            StartCoroutine(SSSSRButtonAnim());
        }
        else
        {
            GameManager.Instance.Phase = StageState.General;
            ToggleMenuDimmer(false);
        }
    }

    IEnumerator SSSSRButtonAnim()
    {
        yield return new WaitForSeconds(2);
        EarnedPanelBtn.SetActive(true);
    }
    #endregion

    #region 정책 관련 메소드
    public void UpdatePolicy()
    {
        for (int i = 0; i < PolicyDisplayList.Count; i++)
            Destroy(PolicyDisplayList[i]);
        PolicyDisplayList.Clear();

        List<PolicyDataCore> policyList = GameManager.Instance.MakePolicyList();
        for (int i = 0; i < policyList.Count; i++)
        {
            GameObject newObj = Instantiate(PolicyTemplate);
            PolicyData newPolicy = newObj.GetComponent<PolicyData>();
            newPolicy.NameText.text = policyList[i].Name;
            newPolicy.DescriptionText.text = policyList[i].Description;
            newPolicy.MoneyDeltaValue = policyList[i].MoneyDeltaValue;
            newPolicy.ApprovalDeltaValue = policyList[i].ApprovalDeltaValue;
            newPolicy.PopulationDeltaValue = policyList[i].PopulationDeltaValue;
            newPolicy.GutPriceDeltaValue = policyList[i].GutPriceDeltaValue;
            newPolicy.IsPercentPoint = policyList[i].IsPercentPoint;
            newObj.GetComponent<RectTransform>().SetParent(PolicyListBody);
            newObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            newObj.SetActive(true);
            newPolicy.DisplayValueInfo();
            PolicyDisplayList.Add(newObj);
        }
    }

    public void SkipPolicy()
    {
        GameManager.Instance.SkipPolicy();

        PolicyDimmer.SetActive(true);
        PolicyAvailable.SetActive(false);
        UpdatePolicyGachaCount();
    }

    public void UpdatePolicyGachaCount()
    {
        PolicyGachaText.text = (GameManager.Instance.PolicyThreshold - GameManager.Instance.PolicyCount).ToString();

        if (GameManager.Instance.PolicyCount >= GameManager.Instance.PolicyThreshold)
        {
            PolicyDimmer.SetActive(false);
            PolicyAvailable.SetActive(true);
            UpdatePolicy();
        }
        else
        {
            PolicyDimmer.SetActive(true);
            PolicyAvailable.SetActive(false);
        }
    }
    #endregion

    public void Save()
    {
        GameManager.Instance.SaveCurrentGame();
    }

    public void UpdateStage()
    {
        StageNameText.text = GameManager.Instance.UpdateAndGetStage();

        ChangePanel(0);
        if (GameManager.Instance.StageLevel >= 30)
            EndingPopup.SetActive(true);
        else
            NewStagePopup.SetActive(true);
        ToggleMenuDimmer(true);
    }

    public void ResetGameState() { GameManager.Instance.Phase = StageState.General; }

    public void GameOver()
    {
        GameManager.Instance.Phase = StageState.GameOver;
        GameOverPanel.gameObject.SetActive(true);
    }

    public void QuitStage()
    {
        SceneChanger.LoadScene("Title Screen");
    }
}