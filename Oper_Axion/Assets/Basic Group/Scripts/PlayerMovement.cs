using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charaCon;
    
    private PlayerStatus playerStatus;

    private Vector3 moveForce;

    private float moveSpeed;

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private void Awake()
    {
        charaCon = GetComponent<CharacterController>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (!charaCon.isGrounded) moveForce.y += playerStatus.Gravity * Time.deltaTime;

        charaCon.Move(moveForce * Time.deltaTime);
    }

    public void MoveDirection(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }

    public void Jump()
    {
        if (charaCon.isGrounded) moveForce.y = playerStatus.JumpForce;
    }
}
