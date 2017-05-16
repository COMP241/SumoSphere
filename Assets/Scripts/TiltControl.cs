using UnityEngine;

public class TiltControl : MonoBehaviour
{
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
            targetRotation = Quaternion.Euler(-20 * vertical, 0, 20 * horizontal);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 30f);
        Physics.gravity = targetRotation * Vector3.down * 9.81f;
    }

    private static Quaternion ChangeAttitude(Quaternion q)
    {
        return new Quaternion(-q.y, 0.0f, q.x, 1.0f);
    }
}
