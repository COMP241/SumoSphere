using UnityEngine;

public class TiltControl : MonoBehaviour
{
    [SerializeField] private Transform pivot;

    private void Start()
    {
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        if (GameController.timeRunning)
        {
            if (SystemInfo.supportsGyroscope)
            {
                Vector3 grav = Input.gyro.gravity.normalized;
                Physics.gravity = 9.81f * new Vector3(grav.x, grav.z, grav.y);
                pivot.rotation = Quaternion.LookRotation(Physics.gravity);
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                Quaternion targetRotation = Quaternion.Euler(-30 * vertical, 0, 30 * horizontal);
                Physics.gravity = targetRotation * Vector3.down * 9.81f;
            }
        }
        else
        {
            Physics.gravity = Vector3.zero;
        }
    }

    public static void ResetGravity()
    {
        Physics.gravity = Vector3.down * 9.81f;
    }
}
