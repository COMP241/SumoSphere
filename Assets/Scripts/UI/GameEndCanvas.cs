using UnityEngine;
using UnityEngine.UI;

public class GameEndCanvas : MonoBehaviour
{
    private static GameEndCanvas instance;

    // Editor Fields
    [SerializeField] private Text timeText;
    [SerializeField] private ScoreSubmit scoreSubmit;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<GameEndCanvas>();
        else
            Destroy(gameObject);
        Hide();
    }

    public static void SetTime(float seconds)
    {
        instance.timeText.text = seconds.ToString("0.00");
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }

    public static void Show()
    {
        instance.scoreSubmit.Enable();
        instance.gameObject.SetActive(true);
    }
}
