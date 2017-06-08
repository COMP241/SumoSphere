using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownCanvas : MonoBehaviour
{

    private static CountdownCanvas instance;
    [SerializeField] private Animator countdownAnimator;
    [SerializeField] private SoundPlayer countdownSound;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<CountdownCanvas>();
        else
            Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }

    public static void Show()
    {
        instance.countdownAnimator.Play("Countdown", -1, 0f);
        instance.gameObject.SetActive(true);
    }
}
