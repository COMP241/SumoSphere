using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    private static UICanvas instance;

    // Editor Fields
    [SerializeField] private Text displayText;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<UICanvas>();
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

    public static void Clear()
    {
        instance.gameObject.SetActive(false);
    }
}
