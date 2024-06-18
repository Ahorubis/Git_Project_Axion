using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour
{
    private PlayerRotation playerRotation;
    private PlayerMovement playerMovement;
    private PlayerStatus playerStatus;
    private AudioSource audioSource;

    private AudioSource childAudio;
    private Animator gunAnime;

    private GameHUD gameHUD;
    private GameSystem gameSystem;
    private Camera mainCamera;
    private EnemyStatus enemyStatus;

    private float maxFireRate = 0.15f;
    private float currentFireRate;

    private int bulletReset;
    private int randomSpawn;
    private int playerHealthReset;

    public AudioSource ChildAudio => childAudio;

    private void Awake()
    {
        playerRotation = GetComponent<PlayerRotation>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
        audioSource = GetComponent<AudioSource>();

        childAudio = GetComponentInChildren<AudioSource>();
        gunAnime = GetComponentInChildren<Animator>();

        gameHUD = GameObject.Find("PlayerHUD").GetComponent<GameHUD>();
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        mainCamera = Camera.main;

        playerStatus.PlayerRunning = false;
        playerStatus.DamageCooldown = false;

        playerHealthReset = playerStatus.HealthPoint;
    }

    private void Start()
    {
        bulletReset = playerStatus.Bullet;

        playerStatus.MuzzleFlame.SetActive(false);

        randomSpawn = Random.Range(0, playerStatus.PlayerSpawnPoints.Length);
        transform.position = playerStatus.PlayerSpawnPoints[randomSpawn].position;
    }

    private void Update()
    {
        DeltaRotation();
        DeltaMoving();
        DeltaJump();
        GetFireOrItem();
        Reloding();

        //히든키 - 피격효과 및 체력감소(패배)
        if (Input.GetKeyDown(KeyCode.U)) StartCoroutine(gameHUD.OnBloodScreen(playerHealthReset));

        //히든키 - 시간만료(승리)
        if (gameHUD.TimerMinute > 0 && Input.GetKeyDown(KeyCode.I))
        {
            childAudio.PlayOneShot(playerStatus.AudioHidden);
            gameHUD.TimerSecond = 5;
            gameHUD.TimerMinute = 0;
        }

        //우주선 추락할 경우
        if (transform.position.y <= -50) StartCoroutine(gameHUD.OnBloodScreen(playerHealthReset));

        //생존실패 - 베드엔딩
        if (playerStatus.HealthPoint <= 0) gameSystem.FailSurvive();

        //생존성공 - 굿엔딩
        if (gameHUD.TimerMinute <= 0 && (int)gameHUD.TimerSecond <= 0) gameSystem.SuccessSurvive();
    }

    //카메라 회전
    private void DeltaRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (gameHUD.PauseStatus == 0) playerRotation.MouseRotation(mouseX, mouseY);
    }

    //캐릭터 이동
    private void DeltaMoving()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            bool running = false;

            if (playerStatus.RunCooldown == false) running = Input.GetKey(playerStatus.KeyCodeRun);

            playerMovement.MoveSpeed = running == true ? playerStatus.RunSpeed : playerStatus.WalkSpeed;
            audioSource.clip = running == true ? playerStatus.AudioRun : playerStatus.AudioWalk;

            if (audioSource.isPlaying == false)
            {
                if (gameHUD.PauseStatus == 1) return;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (playerMovement.MoveSpeed == playerStatus.RunSpeed) playerStatus.PlayerRunning = true;
            else playerStatus.PlayerRunning = false;
        }

        else
        {
            playerMovement.MoveSpeed = 0;
            audioSource.loop = false;
            playerStatus.PlayerRunning = false;
        }

        playerMovement.MoveDirection(new Vector3(x, 0, z));
    }

    //점프
    private void DeltaJump()
    {
        if (Input.GetKeyDown(playerStatus.KeyCodeJump))
        {
            childAudio.PlayOneShot(playerStatus.AudioJump);
            playerMovement.Jump();
        }
    }

    //총알 발사 및 아이템 획득
    private void GetFireOrItem() 
    {
        Mathf.Clamp(currentFireRate, 0, maxFireRate);
        currentFireRate += Time.deltaTime;

        gunAnime.SetBool("Fire", false);

        Ray ray = mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));      //플레이어 손 제외

        gameHUD.Item.SetActive(false);

        if (gameHUD.PauseStatus != 0) return;

        //일정 범위 내에서 아이템 발견 시
        if (Physics.Raycast(ray, out hit, playerStatus.LimitItemDistance, layerMask))
        {
            //탄창 획득
            if (hit.transform.CompareTag("Ammo"))
            {
                gameHUD.Item.SetActive(true);

                if (Input.GetKeyDown(playerStatus.KeyCodeItem))
                {
                    childAudio.PlayOneShot(playerStatus.AudioItem);
                    playerStatus.Magazine++;
                    hit.transform.gameObject.SetActive(false);
                    gameHUD.Item.SetActive(false);
                }
            }

            //응급상자 획득
            if (hit.transform.CompareTag("Heal"))
            {
                gameHUD.Item.SetActive(true);

                if (Input.GetKeyDown(playerStatus.KeyCodeItem))
                {
                    childAudio.PlayOneShot(playerStatus.AudioItem);
                    playerStatus.HealthPoint += playerHealthReset / 4;

                    if (playerStatus.HealthPoint >= playerHealthReset) playerStatus.HealthPoint = playerHealthReset;

                    hit.transform.gameObject.SetActive(false);
                    gameHUD.Item.SetActive(false);
                }
            }
        }

        //총알 발사
        if (Input.GetMouseButton(0) && currentFireRate > maxFireRate)
        {
            if ((Input.GetAxisRaw("Vertical") > 0 && playerStatus.PlayerRunning == true) || playerStatus.Bullet <= 0) return;

            childAudio.PlayOneShot(playerStatus.AudioShot);     //발사 효과음
            StartCoroutine("OnMuzzleFlashEffect");              //발사 화염효과
            playerStatus.Bullet--;                              //총알 개수 감소
            currentFireRate = 0;                                //연사 간격 초기화
            gameHUD.GetRecoil();                                //조준경 애니메이션

            //사정거리 내에서 피격 위치
            if (Physics.Raycast(ray, out hit, playerStatus.LimitFireDistance, layerMask))
            {
                //적 조준사격
                if (hit.transform.CompareTag("Enemy"))
                {
                    enemyStatus = hit.transform.gameObject.GetComponent<EnemyStatus>();

                    playerStatus.EnemyShot.transform.position = hit.point;
                    playerStatus.EnemyShot.transform.forward = hit.normal;
                    Instantiate(playerStatus.EnemyShot);
                    enemyStatus.EnemyHealth--;
                }

                //탄창 및 체력팩 조준사격
                else if (hit.transform.CompareTag("Ammo") || hit.transform.CompareTag("Heal"))
                {
                    childAudio.PlayOneShot(playerStatus.AudioShotToItem);
                    playerStatus.SpaceshipShot.transform.position = hit.point;
                    playerStatus.SpaceshipShot.transform.forward = hit.normal;
                    Instantiate(playerStatus.SpaceshipShot);
                    hit.transform.gameObject.SetActive(false);
                }

                //이외 조준사격
                else
                {
                    playerStatus.SpaceshipShot.transform.position = hit.point;
                    playerStatus.SpaceshipShot.transform.forward = hit.normal;
                    Instantiate(playerStatus.SpaceshipShot);
                }
            }
        }
    }

    //총알 장전
    private void Reloding()
    {
        if (Input.GetKeyDown(playerStatus.KeyCodeReload) && !Input.GetMouseButton(0) && (playerStatus.Magazine > 0))
        {
            childAudio.PlayOneShot(playerStatus.AudioReload);
            gunAnime.SetTrigger("Reload");
            playerStatus.Magazine--;
            playerStatus.Bullet = bulletReset;
        }
    }

    //투사체 피격
    public void DamageForSpit()
    {
        StartCoroutine(gameHUD.OnBloodScreen(8));
    }

    //화염 효과
    private IEnumerator OnMuzzleFlashEffect()
    {
        playerStatus.MuzzleFlame.SetActive(true);
        gunAnime.SetTrigger("Fire");

        yield return new WaitForSeconds(0.1f);

        playerStatus.MuzzleFlame.SetActive(false);
    }
}
