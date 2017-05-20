using UnityEngine;
using UnityEngine.UI;

class MainCanvas : MonoBehaviour
{
    private static MainCanvas instance;

    // Editor Fields
    [SerializeField] private Text timeText;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<MainCanvas>();
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
        instance.gameObject.SetActive(true);
    }
}
