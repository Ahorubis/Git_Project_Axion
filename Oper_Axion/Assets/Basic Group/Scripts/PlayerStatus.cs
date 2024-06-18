using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerStatus : MonoBehaviour
{
    [Header("Player Spawn Point")]
    [SerializeField] private Transform[] playerSpawnPoints;     //플레이어 스폰지점 그룹

    [Header("Player Fire Effects")]
    [SerializeField] private GameObject muzzleFlame;        //발사 화염 효과
    [SerializeField] private GameObject enemyShot;          //적 피격 효과
    [SerializeField] private GameObject spaceshipShot;      //우주선 피격 효과

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioWalk;       //걷기 음원
    [SerializeField] private AudioClip audioRun;        //달리기 음원
    [SerializeField] private AudioClip audioReload;     //재장전 음원
    [SerializeField] private AudioClip audioShot;       //발사 음원
    [SerializeField] private AudioClip audioItem;       //아이템 획득 음원
    [SerializeField] private AudioClip audioShotToItem; //아이템 명중 음원
    [SerializeField] private AudioClip audioHurt;       //피격 음원
    [SerializeField] private AudioClip audioJump;       //점프 음원
    [SerializeField] private AudioClip audioHidden;     //히든키 음원

    [Header("Player Key")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;        //뛰는 키
    [SerializeField] private KeyCode keyCodeReload = KeyCode.R;             //재장전 키
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;           //점프 키
    [SerializeField] private KeyCode keyCodePause = KeyCode.Escape;         //일시정지 키
    [SerializeField] private KeyCode keyCodeItem = KeyCode.E;               //아이템 획득 키

    [Header("Player Stat")]
    [SerializeField] private float walkSpeed = 4;           //걷기 속도
    [SerializeField] private float runSpeed = 7;            //달리기 속도
    [SerializeField] private float jumpForce = 5;           //점프 힘
    [SerializeField] private float gravity = -10;           //중력
    [SerializeField] private float limitFireDistance = 90;  //총알 사거리
    [SerializeField] private float runTimer = 5;            //달리기 제한시간
    [SerializeField] private float limitItemDistance = 15;  //탄창보급 사거리
    [SerializeField] private float pushForce = 2;           //충돌 시 밀어내는 힘

    [Header("Mouse Rotation Sensitivity")]
    [SerializeField] private float rotationCameraXSpeed = 6;        //마우스 X축 민감도
    [SerializeField] private float rotationCameraYSpeed = 3;        //마우스 Y축 민감도

    [Header("Player Resources")]
    [SerializeField] private int healthPoint = 100;     //체력
    [SerializeField] private int magazine = 5;          //탄창
    [SerializeField] private int bullet = 30;           //탄창 당 총알 개수


    private bool playerRunning;
    private bool runCooldown;
    private bool damageCooldown;


    public Transform[] PlayerSpawnPoints => playerSpawnPoints;

    public GameObject MuzzleFlame => muzzleFlame;
    public GameObject EnemyShot => enemyShot;
    public GameObject SpaceshipShot => spaceshipShot;

    public AudioClip AudioWalk => audioWalk;
    public AudioClip AudioRun => audioRun;
    public AudioClip AudioReload => audioReload;
    public AudioClip AudioShot => audioShot;
    public AudioClip AudioItem => audioItem;
    public AudioClip AudioShotToItem => audioShotToItem;
    public AudioClip AudioHurt => audioHurt;
    public AudioClip AudioJump => audioJump;
    public AudioClip AudioHidden => audioHidden;

    public KeyCode KeyCodeRun => keyCodeRun;
    public KeyCode KeyCodeReload => keyCodeReload;
    public KeyCode KeyCodeJump => keyCodeJump;
    public KeyCode KeyCodePause => keyCodePause;
    public KeyCode KeyCodeItem => keyCodeItem;

    public float RotationCameraXSpeed => rotationCameraXSpeed;
    public float RotationCameraYSpeed => rotationCameraYSpeed;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;
    public float Gravity => gravity;
    public float LimitFireDistance => limitFireDistance;
    public float RunTimer => runTimer;
    public float LimitItemDistance => limitItemDistance;
    public float PushForce => pushForce;

    public int HealthPoint
    {
        get => healthPoint;
        set => healthPoint = value;
    }

    public int Magazine
    {
        get => magazine;
        set => magazine = value;
    }

    public int Bullet
    {
        get => bullet;
        set => bullet = value;
    }


    public bool PlayerRunning
    {
        get => playerRunning;
        set => playerRunning = value;
    }

    public bool RunCooldown
    {
        get => runCooldown;
        set => runCooldown = value;
    }

    public bool DamageCooldown
    {
        get => damageCooldown;
        set => damageCooldown = value;
    }
}
