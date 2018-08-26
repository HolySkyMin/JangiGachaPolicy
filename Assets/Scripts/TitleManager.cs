using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text VersionText;

    private void Start()
    {
        GameManager.Instance.State = GameState.Title;
        VersionText.text = Application.version;
    }

    public void CreateNewGame(int index)
    {
        GameManager.Instance.CreateNewGame(index);
    }

    public void LoadSavedGame(int index)
    {
        GameManager.Instance.LoadSavedGame(index);
    }
}