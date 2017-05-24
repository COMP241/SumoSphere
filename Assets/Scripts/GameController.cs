using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    // Generated Fields
    private static Vector3 spawnPoint = Vector3.up * 0.5f;
    private static float time = 0f;
    private static bool timeRunning = false;

    // Editor Fields
    [SerializeField] private Player player;

    void Start()
    {
        if (instance == null)
            instance = GetComponent<GameController>();
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (timeRunning)
        {
            time += Time.deltaTime;
            MainCanvas.SetTime(time);
        }
    }

    public static void SetSpawn(Vector3 point)
    {
        spawnPoint = point;
    }

    public static void Begin()
    {
        instance.RestartGame();
    }

    public void RestartGame()
    {
        Respawn();
        LevelLoader.SetActive(true);
        GameStartCanvas.Hide();
        MainCanvas.Show();
        GameEndCanvas.Hide();
    }
    
    public static void Respawn()
    {
        time = 0f;
        timeRunning = true;
        instance.player.transform.position = spawnPoint;
        instance.player.ResetVelocity();
    }

    public static void Win()
    {
        timeRunning = false;
        LevelLoader.SetActive(false);
        MainCanvas.Hide();
        GameEndCanvas.Show();
        GameEndCanvas.SetTime(time);
    }

    public void LoadNew()
    {
        LevelLoader.Unload();
        GameStartCanvas.Show();
        GameEndCanvas.Hide();
    }
}
