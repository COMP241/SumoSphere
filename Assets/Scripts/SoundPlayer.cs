using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    private static SoundPlayer instance;

    [Header("Sources")]
    [SerializeField] private AudioSource announcerSource;

    [Header("Music")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerSnapshot musicPlaying, musicMute;
    
    private void Start()
    {
        if (instance == null)
            instance = GetComponent<SoundPlayer>();
        else
            Destroy(gameObject);
    }
    
    public static void PlayAnnouncer(AudioClip clip, float volume)
    {
        instance.announcerSource.PlayOneShot(clip, volume);
    }

    public static void MuteMusic()
    {
        instance.mixer.TransitionToSnapshots(new [] {instance.musicMute}, new [] {1f}, 1f);
    }

    public static void UnmuteMusic()
    {
        instance.mixer.TransitionToSnapshots(new [] {instance.musicPlaying}, new [] {1f}, 1f);
    }
}
