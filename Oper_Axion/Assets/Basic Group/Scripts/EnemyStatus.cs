using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {None = -1, Idle = 0, Wander, Pursuit,}

public class EnemyStatus : MonoBehaviour
{
    [Header("Enemy Ammo & Motion")]
    [SerializeField] private GameObject enemyAmmo;
    [SerializeField] private GameObject enemyDying;

    [Header("Enemy Resources")]
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private float enmeyDelay = 3f;

    [Header("Pursuit")]
    [SerializeField] private float pursuitLimitrange = 12;

    [Header("Enemy Setting Status")]
    [SerializeField] private int enemyHealth = 3;

    private NavMeshAgent navMesh;
    private Transform target;
    
    private PlayerStatus playerStatus;
    private GameHUD gameHUD;

    private EnemyState enemyState = EnemyState.None;

    public int EnemyHealth
    {
        get => enemyHealth;
        set => enemyHealth = value;
    }

    private void Awake()
    {
        target = GameObject.Find("Player").transform;

        navMesh = GetComponent<NavMeshAgent>();

        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameHUD = GameObject.Find("PlayerHUD").GetComponent<GameHUD>();

        navMesh.updateRotation = false;

        if (enemyAmmo != null) StartCoroutine(Spit(enmeyDelay));
    }

    private void OnEnable()
    {
        DeltaState(EnemyState.Idle);
    }

    private void OnDisable()
    {
        StopCoroutine(enemyState.ToString());
        enemyState = EnemyState.None;
    }

    private void Update()
    {
        if (enemyHealth <= 0) StartCoroutine("DyingMotion");
    }

    private void DeltaState(EnemyState state)
    {
        if (enemyState == state) return;

        StopCoroutine(enemyState.ToString());
        enemyState = state;
        StartCoroutine(enemyState.ToString());
    }

    private void DistanceAndState()
    {
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= pursuitLimitrange) DeltaState(EnemyState.Pursuit);

        else if (distance >= pursuitLimitrange) DeltaState(EnemyState.Wander);
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Player") && playerStatus.DamageCooldown == false) StartCoroutine(gameHUD.OnBloodScreen(5));
    }

    private IEnumerator Idle()
    {
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            DistanceAndState();

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = Random.Range(1, 5);

        yield return new WaitForSeconds(changeTime);

        DeltaState(EnemyState.Wander);
    }

    private IEnumerator Wander()
    {
        float current = 0;
        float maxTime = 10;

        navMesh.speed = enemySpeed;

        navMesh.SetDestination(CalculateWanderPosition());

        Vector3 to = new Vector3(navMesh.destination.x, 0, navMesh.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            current += Time.deltaTime;

            to = new Vector3(navMesh.destination.x, 0, navMesh.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
            if ((to - from).sqrMagnitude < 0.01f || current >= maxTime)
            {
                DeltaState(EnemyState.Idle);
            }

            DistanceAndState();

            yield return null;
        }
    }

    private IEnumerator Pursuit()
    {
        while (true)
        {
            navMesh.SetDestination(target.position);

            LookRotationToTarget();
            DistanceAndState();

            yield return null;
        }
    }

    //적의 투사체
    private IEnumerator Spit(float timerMax)
    {
        float timer = 0;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= timerMax)
            {
                enemyAmmo.transform.position = transform.position;
                enemyAmmo.transform.rotation = transform.rotation;
                Instantiate(enemyAmmo);
                timer = 0;
            }

            yield return null;
        }
    }

    private IEnumerator DyingMotion()
    {
        enemyDying.transform.position = transform.position;
        enemyDying.transform.rotation = transform.rotation;
        Instantiate(enemyDying);

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRad = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJittermax = 360;

        Vector3 rangePos = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100f;

        wanderJitter = Random.Range(wanderJitterMin, wanderJittermax);

        Vector3 targetPos = transform.position + AngleSetting(wanderRad, wanderJitter);

        targetPos.x = Mathf.Clamp(targetPos.x, rangePos.x - rangeScale.x * 0.5f, rangePos.x + rangeScale.x * 0.5f);
        targetPos.y = 0f;
        targetPos.z = Mathf.Clamp(targetPos.z, rangePos.z - rangeScale.z * 0.5f, rangePos.z + rangeScale.z * 0.5f);

        return targetPos;
    }

    private Vector3 AngleSetting(float rad, int angle)
    {
        Vector3 pos = Vector3.zero;

        pos.x = Mathf.Cos(angle) * rad;
        pos.z = Mathf.Sin(angle) * rad;

        return pos;
    }
}
