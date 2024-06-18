using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorController : MonoBehaviour
{
    public GameObject character;

    public float distance = 5f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector3.Distance(character.transform.position, transform.position) <= distance)
        {
            animator.SetBool("character_nearby", true);
        }
        else
        {
            animator.SetBool("character_nearby", false);
        }
    }
}

