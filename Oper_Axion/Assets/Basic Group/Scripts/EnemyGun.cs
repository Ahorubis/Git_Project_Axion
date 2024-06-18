using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] private GameObject poisoned;

    private PlayerStatus playerStatus;
    private PlayerController playerController;

    private void Awake()
    {
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        StartCoroutine("DyingTimer");
    }

    private void Update()
    {
        transform.Translate(Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerStatus.DamageCooldown == false)
        {
            transform.Translate(Vector3.zero);

            poisoned.transform.position = transform.position;
            poisoned.transform.rotation = transform.rotation;
            Instantiate(poisoned);

            playerController.DamageForSpit();
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceShip"))
        {
            transform.Translate(Vector3.zero);

            poisoned.transform.position = transform.position;
            poisoned.transform.rotation = transform.rotation;
            Instantiate(poisoned);

            Destroy(gameObject);
        }

        
    }

    private IEnumerator DyingTimer()
    {
        yield return new WaitForSeconds(4);
        Instantiate(poisoned);
        Destroy(gameObject);
    }
}
