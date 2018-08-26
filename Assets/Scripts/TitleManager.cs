using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.State = GameState.Title;
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