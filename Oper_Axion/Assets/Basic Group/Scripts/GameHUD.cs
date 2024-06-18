using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Image bloodScreen;
    [SerializeField] private AnimationCurve curveScreen;

    [Header("Stamina")]
    [SerializeField] private Image runningTimeBar;
    [SerializeField] private float staminaTimer = 5;

    [Header("Player Status Info")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI magazineHUD;
    [SerializeField] private TextMeshProUGUI bulletHUD;

    public GameObject Item;

    [Header("Reticle Images")]
    [SerializeField] private RectTransform[] reticles;

    [Header("Pause Window")]
    [SerializeField] private GameObject pauseWindow;

    [Header("Playing Timer")]
    [Range(0, 60)] [SerializeField] private float settingTimerSecond = 0;
    [SerializeField] private int settingTimerMinute = 5;

    [Header("Current Time")]
    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI time; 

    [Header("InGame Timer")]
    public TextMeshProUGUI TimerMinuteText;
    public TextMeshProUGUI TimerSecondText;

    private PlayerStatus playerStatus;
    private PlayerController playerCon;

    private Vector3[] reticlesReset;

    private float reticleSpread = 15f;
    private float staminaTimerReset;
    private float secondReset;

    private int healthReset;
    private int pauseStatus;
    private int minuteReset;
    private int bulletReset;

    public float TimerSecond { get => settingTimerSecond; set => settingTimerSecond = value; }

    public int TimerMinute { get => settingTimerMinute; set => settingTimerMinute = value; }
    public int PauseStatus { get => pauseStatus; private set => pauseStatus = value; }

    private void Awake()
    {
        //게임시작 시, 마우스 커서 숨김
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        playerCon = GameObject.Find("Player").GetComponent<PlayerController>();

        Item.SetActive(false);

        reticlesReset = new Vector3[reticles.Length];

        staminaTimerReset = staminaTimer;
        secondReset = settingTimerSecond;

        pauseStatus = 0;
        minuteReset = settingTimerMinute;

        bulletReset = playerStatus.Bullet;
    }

    private void Start()
    {
        for (int i = 0; i < reticles.Length; i++) reticlesReset[i] = reticles[i].position;

        healthReset = playerStatus.HealthPoint;

        runningTimeBar.fillAmount = 1;
        healthBar.fillAmount = 1;

        TimerSecondText.text = string.Format("{0:D2}", settingTimerMinute);
        TimerMinuteText.text = string.Format("{0:D2}", (int)settingTimerSecond);

        Status();
    }

    private void Update()
    {
        Status();
        PauseWindowInGame();
        GameTimer();
        StaminaHUD();
        HealthHUD();
        GetCurrentTime();

        if (Input.GetKeyDown(KeyCode.Escape)) pauseStatus++;
        if (bloodScreen.color.a == 1) playerStatus.DamageCooldown = false;
    }

    //탄창 및 탄알 표시
    private void Status()
    {
        magazineHUD.text = $"{playerStatus.Magazine}";
        bulletHUD.text = $"<size=90><color=#FFC80F>{playerStatus.Bullet}</color></size>  {bulletReset}";
    }

    //일시정지 창
    private void PauseWindowInGame()
    {
        pauseStatus %= 2;

        if (pauseStatus == 0)
        {
            //게임시작 시, 마우스 커서 숨김
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;
            pauseWindow.SetActive(false);
        }

        else if (pauseStatus == 1)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
            pauseWindow.SetActive(true);
        }
    }

    //게임 타이머 설정 및 표시
    private void GameTimer()
    {
        settingTimerSecond -= Time.deltaTime;

        if (settingTimerMinute > 0 && settingTimerSecond < 0f)
        {
            settingTimerSecond = 60f;
            settingTimerMinute--;
        }

        if (settingTimerMinute <= 0) TimerMinuteText.text = string.Format("{0:D2}", 00);
        else TimerMinuteText.text = string.Format("{0:D2}", settingTimerMinute);

        if ((int)settingTimerSecond == 60) TimerSecondText.text = string.Format("{0:D2}", 00);
        else TimerSecondText.text = string.Format("{0:D2}", (int)settingTimerSecond);
    }

    //달리기 스태미나
    private void StaminaHUD()
    {
        float percent = staminaTimer / staminaTimerReset;

        if (playerStatus.PlayerRunning == true)
        {
            staminaTimer -= Time.deltaTime;
            runningTimeBar.fillAmount = Mathf.Lerp(runningTimeBar.fillAmount, percent, staminaTimer);

            if (Input.GetKey(playerStatus.KeyCodeRun) && staminaTimer < 0.05f) playerStatus.RunCooldown = true;
        }

        else
        {
            if (playerStatus.RunCooldown == false)
            {
                if (staminaTimer < staminaTimerReset)
                {
                    staminaTimer += Time.deltaTime / 1.2f;
                    runningTimeBar.fillAmount = Mathf.Lerp(runningTimeBar.fillAmount, percent, staminaTimer);
                }
            }

            else
            {
                if (staminaTimer < staminaTimerReset)
                {
                    staminaTimer += Time.deltaTime / 1.8f;
                    runningTimeBar.fillAmount = Mathf.Lerp(runningTimeBar.fillAmount, percent, staminaTimer);
                }

                else
                {
                    staminaTimer = staminaTimerReset;
                    runningTimeBar.fillAmount = 1;
                    playerStatus.RunCooldown = false;
                }
                
            }
        }
    }

    //체력바
    private void HealthHUD()
    {
        float percent = (float)playerStatus.HealthPoint / (float)healthReset;
        healthBar.fillAmount = percent;
    }

    //현재시각
    private void GetCurrentTime()
    {
        date.text = DateTime.Now.ToString("yyyy.MM.dd.");
        time.text = DateTime.Now.ToString("HH:mm:ss");

    }

    //벌어지는 조준점
    public void GetRecoil()
    {
        for (int i = 0; i < reticles.Length; i++) reticles[i].position = reticlesReset[i];

        StopCoroutine("SetReticle");

        reticles[0].position += new Vector3(0, reticleSpread, 0);     //상 조준점
        reticles[1].position += new Vector3(0, -reticleSpread, 0);    //하 조준점
        reticles[2].position += new Vector3(-reticleSpread, 0, 0);    //좌 조준점
        reticles[3].position += new Vector3(reticleSpread, 0, 0);     //우 조준점

        StartCoroutine("SetReticle");
    }

    //조준점 회복
    private IEnumerator SetReticle()
    {
        float min = 0;
        float max = 1;

        yield return new WaitForSeconds(0.5f);

        while (min <= max)
        {
            float t = min / max;

            for (int i = 0; i < reticles.Length; i++) reticles[i].position = Vector3.Lerp(reticles[i].position, reticlesReset[i], t);

            min += Time.deltaTime;
            yield return null;
        }
    }

    //피격 그래픽 효과
    public IEnumerator OnBloodScreen(int damage)
    {
        float timer = 0;

        playerCon.ChildAudio.PlayOneShot(playerStatus.AudioHurt);
        playerStatus.DamageCooldown = true;
        playerStatus.HealthPoint -= damage;

        while (true)
        {
            timer += Time.deltaTime;

            Color color = bloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveScreen.Evaluate(timer));
            bloodScreen.color = color;

            if (color.a <= 0.1f)
            {
                color.a = 0;
                playerStatus.DamageCooldown = false;
                yield break;
            }

            yield return null;
        }
    }
}
