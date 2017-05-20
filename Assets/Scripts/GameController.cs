using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    // Generated Fields
    private static Vector3 spawnPoint = Vector3.up * 0.5f;

    // Editor Fields
    [SerializeField] private Player player;

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
        instance.RestartGame();
    }

    public void RestartGame()
    {
        Respawn();
        LevelLoader.SetActive(true);
        GameEndCanvas.Hide();
    }
    
    public static void Respawn()
    {
        instance.player.transform.position = spawnPoint;
        instance.player.ResetVelocity();
    }

    public static void Win()
    {
        LevelLoader.SetActive(false);
        GameEndCanvas.Show();
    }

    public void LoadNew()
    {
        LevelLoader.Unload();
        GameEndCanvas.Hide();
        GameStartCanvas.Show();
    }
}
