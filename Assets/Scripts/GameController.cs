using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    // Generated Fields
    private static Vector3 spawnPoint = Vector3.up * 0.5f;
    public static float time = 0f;
    public static bool timeRunning = false;

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
        SoundPlayer.MuteMusic();
        Respawn();
        LevelLoader.SetActive(true);
        timeRunning = false;
        MainCanvas.SetTime(0f);
        GameStartCanvas.Hide();
        GameEndCanvas.Hide();
        CountdownCanvas.Show();
        MainCanvas.Show();
    }

    public static void SetOff()
    {
        timeRunning = true;
    }

    public void Respawn_()
    {
        MainCanvas.SetTime(0f);
        RestartGame();
    }

    public static void Respawn()
    {
        time = 0f;
        instance.player.transform.position = spawnPoint;
        instance.player.ResetVelocity();
    }

    public static void Win()
    {
        SoundPlayer.MuteMusic(5f);
        timeRunning = false;
        LevelLoader.SetActive(false);
        MainCanvas.Hide();
        GameEndCanvas.Show();
        GameEndCanvas.SetTime(time);
    }

    public void LoadNew()
    {
        SoundPlayer.MuteMusic();
        LevelLoader.Unload();
        LevelLoader.SetActive(false);
        MainCanvas.Hide();
        GameStartCanvas.Show();
        GameEndCanvas.Hide();
    }
}
