using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject text;

    public static string nextScene;

    private void Awake()
    {
        Time.timeScale = 1;
        progressBar.fillAmount = 0;

        text.SetActive(false);
        StartCoroutine("LoadScene");
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress) { timer = 0f; }
            }

            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    text.SetActive(true);

                    if (text.activeSelf == true && Input.anyKeyDown)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }

            Debug.Log(progressBar.fillAmount);
        }
    }
}