using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCountdown : MonoBehaviour {

    private CountdownCanvas CDS;
    [SerializeField] AudioClip clip3, clip2, clip1, clipGo, backgroundMusic;

    public void SetCountdownNow()
    {
        GameController.SetOff();
        CountdownCanvas.Hide();
        //plays background music
        SoundPlayer.PlaySFX(backgroundMusic, 1f);
    }

    public void play3()
    {
        SoundPlayer.PlaySFX(clip3, 1f);
    }

    public void play2()
    {
        SoundPlayer.PlaySFX(clip2, 1f);
    }

    public void play1()
    {
        SoundPlayer.PlaySFX(clip1, 1f);
    }

    public void playGo()
    {
        SoundPlayer.PlaySFX(clipGo, 1f);
    }

    //public void playBackground()
    //{
    //    SoundPlayer.PlaySFX(backgroundMusic, 1f);
    //}
}
