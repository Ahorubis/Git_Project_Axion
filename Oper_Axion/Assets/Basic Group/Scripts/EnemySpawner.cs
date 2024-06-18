using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Info")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectRadius = 20;

    [Header("Enemy Spawn Timer")]
    [SerializeField] private float spawnTimer = 2.4f;

    private Collider[] Colls;

    private Vector3 spawnPoint;

    private float spawnTimerReset;

    private void Awake()
    {
        spawnTimerReset = spawnTimer;
        spawnPoint = transform.position;
    }

    private void Update()
    {
        DetectPlayerAndSpawn();
    }

    private void DetectPlayerAndSpawn()
    {
        Colls = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);

        if (Colls.Length != 0)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                int index = Random.Range(0, enemies.Length);

                enemies[index].transform.position = spawnPoint;
                Instantiate(enemies[index]);

                spawnTimer = spawnTimerReset;
            }
        }

        else spawnTimer = spawnTimerReset;
    }
}
