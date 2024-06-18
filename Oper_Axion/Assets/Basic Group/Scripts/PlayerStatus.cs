using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerStatus : MonoBehaviour
{
    [Header("Player Spawn Point")]
    [SerializeField] private Transform[] playerSpawnPoints;     //�÷��̾� �������� �׷�

    [Header("Player Fire Effects")]
    [SerializeField] private GameObject muzzleFlame;        //�߻� ȭ�� ȿ��
    [SerializeField] private GameObject enemyShot;          //�� �ǰ� ȿ��
    [SerializeField] private GameObject spaceshipShot;      //���ּ� �ǰ� ȿ��

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioWalk;       //�ȱ� ����
    [SerializeField] private AudioClip audioRun;        //�޸��� ����
    [SerializeField] private AudioClip audioReload;     //������ ����
    [SerializeField] private AudioClip audioShot;       //�߻� ����
    [SerializeField] private AudioClip audioItem;       //������ ȹ�� ����
    [SerializeField] private AudioClip audioShotToItem; //������ ���� ����
    [SerializeField] private AudioClip audioHurt;       //�ǰ� ����
    [SerializeField] private AudioClip audioJump;       //���� ����
    [SerializeField] private AudioClip audioHidden;     //����Ű ����

    [Header("Player Key")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;        //�ٴ� Ű
    [SerializeField] private KeyCode keyCodeReload = KeyCode.R;             //������ Ű
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;           //���� Ű
    [SerializeField] private KeyCode keyCodePause = KeyCode.Escape;         //�Ͻ����� Ű
    [SerializeField] private KeyCode keyCodeItem = KeyCode.E;               //������ ȹ�� Ű

    [Header("Player Stat")]
    [SerializeField] private float walkSpeed = 4;           //�ȱ� �ӵ�
    [SerializeField] private float runSpeed = 7;            //�޸��� �ӵ�
    [SerializeField] private float jumpForce = 5;           //���� ��
    [SerializeField] private float gravity = -10;           //�߷�
    [SerializeField] private float limitFireDistance = 90;  //�Ѿ� ��Ÿ�
    [SerializeField] private float runTimer = 5;            //�޸��� ���ѽð�
    [SerializeField] private float limitItemDistance = 15;  //źâ���� ��Ÿ�
    [SerializeField] private float pushForce = 2;           //�浹 �� �о�� ��

    [Header("Mouse Rotation Sensitivity")]
    [SerializeField] private float rotationCameraXSpeed = 6;        //���콺 X�� �ΰ���
    [SerializeField] private float rotationCameraYSpeed = 3;        //���콺 Y�� �ΰ���

    [Header("Player Resources")]
    [SerializeField] private int healthPoint = 100;     //ü��
    [SerializeField] private int magazine = 5;          //źâ
    [SerializeField] private int bullet = 30;           //źâ �� �Ѿ� ����


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
