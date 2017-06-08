using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private static SoundPlayer instance;

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<SoundPlayer>();
        else
            Destroy(gameObject);
    }

    public static void PlaySFX(AudioClip clip, float volume)
    {
        instance.sfxSource.PlayOneShot(clip, volume);
    }
}
