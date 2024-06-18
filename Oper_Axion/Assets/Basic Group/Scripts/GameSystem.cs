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

    //���� ���� �� �����
    public void StartToGame()
    {
        SceneController.LoadScene("AxionSpaceship");
    }

    //���� ����
    public void ExitGame()
    {
        Application.Quit();
    }

    //����ȭ������ �����
    public void ResetGame()
    {
        SceneManager.LoadScene("StartScreen");
    }

    //��Ʈ�� ������� ����
    public void GameIntroStart()
    {
        SceneManager.LoadScene("IntroScreen");
    }

    //���� �¸�ȭ��
    public void SuccessSurvive()
    {
        SceneManager.LoadScene("GoodEndingScreen");
    }

    //���� �й�ȭ��
    public void FailSurvive()
    {
        SceneManager.LoadScene("BadEndingScreen");
    }

    //���� �̽��Ϳ��� ����
    public void EasterEgg()
    {
        hiddenIndex++;
        if (hiddenIndex >= 5) SceneManager.LoadScene("HiddenScreen");
    }
}
