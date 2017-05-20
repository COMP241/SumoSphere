using UnityEngine;
using UnityEngine.UI;

public class GameStartCanvas : MonoBehaviour
{
    private static GameStartCanvas instance;

    // Editor Fields
    [SerializeField] private Text displayText;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<GameStartCanvas>();
        else
            Destroy(gameObject);
    }

    public static void DisplayError(string message)
    {
        instance.displayText.color = Color.red;
        instance.displayText.text = message;
    }

    public static void DisplayInfo(string message)
    {
        instance.displayText.color = Color.black;
        instance.displayText.text = message;
    }

    public static void Hide()
    {
        instance.displayText.text = "";
        instance.gameObject.SetActive(false);
    }

    public static void Show()
    {
        instance.gameObject.SetActive(true);
    }
}
