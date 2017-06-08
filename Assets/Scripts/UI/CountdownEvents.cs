using UnityEngine;

public class CountdownEvents : MonoBehaviour {

    private CountdownCanvas CDS;
    [SerializeField] private AudioClip clip3, clip2, clip1, clipGo;

    public void EndCountdown()
    {
        GameController.SetOff();
        CountdownCanvas.Hide();
    }

    public void Play3()
    {
        SoundPlayer.PlayAnnouncer(clip3, 1f);
    }

    public void Play2()
    {
        SoundPlayer.PlayAnnouncer(clip2, 1f);
    }

    public void Play1()
    {
        SoundPlayer.PlayAnnouncer(clip1, 1f);
        SoundPlayer.UnmuteMusic();
    }

    public void PlayGo()
    {
        SoundPlayer.PlayAnnouncer(clipGo, 1f);
    }
}
