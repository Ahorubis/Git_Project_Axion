using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private AudioClip audioIntro;

    private CanvasGroup introCanvas;
    private AudioSource audioSource;

    private void Awake()
    {
        introCanvas = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        introCanvas.alpha = 1;
    }

    private void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(1.5f));
    }

    public IEnumerator FadeTextToFullAlpha(float t)
    {
        introCanvas.alpha = 0;
        while (introCanvas.alpha < 1.0f)
        {
            introCanvas.alpha += Time.deltaTime / t;
            yield return null;
        }

        introCanvas.alpha = 1;
        audioSource.PlayOneShot(audioIntro);
        yield return new WaitForSeconds(2);

        while (introCanvas.alpha > 0.0f)
        {
            introCanvas.alpha -= Time.deltaTime / t;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("StartScreen");
    }
}
