using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    public Animator SceneAnimation;
    public GameObject LoadingText;

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

    IEnumerator SceneLoadingAction(string sceneName)
    {
        SceneAnimation.Play("SceneClose");
        yield return new WaitForSeconds(0.34f);
        AsyncOperation loadingAction = SceneManager.LoadSceneAsync(sceneName);
        loadingAction.allowSceneActivation = false;
        LoadingText.SetActive(true);
        while (!loadingAction.isDone)
        {
            if (loadingAction.progress >= 0.9f)
                loadingAction.allowSceneActivation = true;
            yield return null;
        }
        LoadingText.SetActive(false);
        SceneAnimation.Play("SceneOpen");
    }

    public static void LoadScene(string sceneName)
    {
        Instance.StartCoroutine(Instance.SceneLoadingAction(sceneName));
    }
}