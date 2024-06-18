using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    private int hiddenIndex = 0;

    public static GameSystem System;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Application.targetFrameRate = 60;
        hiddenIndex = 0;

        if (System != null)
        {
            Destroy(gameObject);
            return;
        }

        System = this;
    }

    //게임 시작 및 재시작
    public void StartToGame()
    {
        SceneController.LoadScene("AxionSpaceship");
    }

    //게임 종료
    public void ExitGame()
    {
        Application.Quit();
    }

    //메인화면으로 재시작
    public void ResetGame()
    {
        SceneManager.LoadScene("StartScreen");
    }

    //인트로 영상부터 시작
    public void GameIntroStart()
    {
        SceneManager.LoadScene("IntroScreen");
    }

    //게임 승리화면
    public void SuccessSurvive()
    {
        SceneManager.LoadScene("GoodEndingScreen");
    }

    //게임 패배화면
    public void FailSurvive()
    {
        SceneManager.LoadScene("BadEndingScreen");
    }

    //게임 이스터에그 진입
    public void EasterEgg()
    {
        hiddenIndex++;
        if (hiddenIndex >= 5) SceneManager.LoadScene("HiddenScreen");
    }
}
