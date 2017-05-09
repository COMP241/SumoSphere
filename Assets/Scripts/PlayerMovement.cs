using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 rotationAxis = new Vector3(vertical, 0, -horizontal);
        rb.AddTorque(rotationAxis);
    }
}
