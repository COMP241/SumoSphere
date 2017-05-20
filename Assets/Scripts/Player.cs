using UnityEngine;

public class Player : MonoBehaviour
{
    // Generated Fields
    private Rigidbody rb;

    //Editor fields
    [SerializeField] private float resetY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y < resetY)
            Respawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
            Respawn();
    }

    private void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GameController.Respawn();
    }
}
