using UnityEngine;

class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject[] toggleOnPause;

    public void Pause()
    {
        bool pausing = Time.timeScale > 0f;
        foreach (GameObject o in toggleOnPause)
            o.SetActive(!o.activeSelf);
        Time.timeScale = pausing ? 0f : 1f;

        if (pausing)
            SoundPlayer.MuteMusic(0f);
        else
            SoundPlayer.UnmuteMusic(0f);
    }

    public void End()
    {
        foreach (GameObject o in toggleOnPause)
            o.SetActive(!o.activeSelf);
        Time.timeScale = 1f;

        SoundPlayer.MuteMusic(0f);
    }
}
