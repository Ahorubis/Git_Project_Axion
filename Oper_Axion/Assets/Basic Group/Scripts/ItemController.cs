using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Range(0, 120)][SerializeField] private float rebornTimer = 60;

    private GameObject item;

    private float rebornTimerReset;

    private void Awake()
    {
        item = transform.GetChild(0).gameObject;
        rebornTimerReset = rebornTimer;
    }

    private void Update()
    {
        if (item.activeSelf == false)
        {
            if (rebornTimer == 0) Destroy(gameObject);

            else
            {
                rebornTimer -= Time.deltaTime;

                if (rebornTimer <= 0)
                {
                    item.SetActive(true);
                    rebornTimer = rebornTimerReset;
                }
            }
        }
    }
}
