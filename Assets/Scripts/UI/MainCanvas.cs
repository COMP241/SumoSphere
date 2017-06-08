using UnityEngine;
using UnityEngine.UI;

class MainCanvas : MonoBehaviour
{
    private static MainCanvas instance;

    // Editor Fields
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject countdownCanvas;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<MainCanvas>();
        else
            Destroy(gameObject);
        Hide();
    }

    private void Update()
    {
        pauseButton.SetActive(!countdownCanvas.activeSelf);
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
        instance.gameObject.SetActive(true);
    }
}
