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
            targetRotation = Quaternion.Euler(20 * vertical, 0, -20 * horizontal);
        }
        DebugCanvas.SetText(targetRotation.ToString());
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 30f);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    private static Quaternion ChangeAttitude(Quaternion q)
    {
        return new Quaternion(q.y, 0.0f, -q.x, 1.0f);
    }
}
