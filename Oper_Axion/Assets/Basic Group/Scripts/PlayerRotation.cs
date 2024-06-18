using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private PlayerStatus playerStatus;

    private float minX = -80;
    private float maxX = 80;
    private float angleX;
    private float angleY;

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    //회전값 계산 및 제한
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    //마우스 회전
    public void MouseRotation(float mouseX, float mouseY)
    {
        angleY += mouseX * playerStatus.RotationCameraYSpeed;
        angleX -= mouseY * playerStatus.RotationCameraXSpeed;

        angleX = ClampAngle(angleX, minX, maxX);

        transform.rotation = Quaternion.Euler(angleX, angleY, 0);
    }
}
