using UnityEngine;

public class TiltControl : MonoBehaviour
{
    [SerializeField] private Transform pivot;

    private void Update()
    {
        Quaternion targetRotation;
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            targetRotation = ChangeAttitude(Input.gyro.attitude);
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            targetRotation = Quaternion.Euler(-30 * vertical, 0, 30 * horizontal);
        }
        Physics.gravity = targetRotation * Vector3.down * 9.81f;
        pivot.rotation = targetRotation;
    }

    public static void ResetGravity()
    {
        Physics.gravity = Vector3.down * 9.81f;
    }

    private static Quaternion ChangeAttitude(Quaternion q)
    {
        return new Quaternion(-q.y, 0.0f, q.x, 1.0f);
    }
}
