using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    // Generated Fields
    private static Vector3 spawnPoint = Vector3.up * 0.5f;

    // Editor Fields
    [SerializeField] private GameObject player;

    void Start()
    {
        if (instance == null)
            instance = GetComponent<GameController>();
        else
            Destroy(gameObject);
    }

    public static void Spawn(Vector3 point)
    {
        spawnPoint = point;
        Respawn();
    }
    
    public static void Respawn()
    {
        instance.player.transform.position = spawnPoint;
    }
}
