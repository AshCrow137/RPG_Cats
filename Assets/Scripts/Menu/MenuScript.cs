
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScrene;
    public  void LoadScene(string sceneName)
    {
        StartCoroutine(SceneLoaderCoroutine(sceneName));
    }
    private IEnumerator SceneLoaderCoroutine(string SceneName)
    {
        if(loadingScrene != null)
        {
            loadingScrene.SetActive(true);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        while(!operation.isDone)
        {
            yield return null;
        }

    }
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    }
