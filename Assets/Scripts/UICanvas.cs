using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    private static UICanvas instance;

    // Editor Fields
    [SerializeField] private Text errorText;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<UICanvas>();
        else
            Destroy(gameObject);
    }

    public static void DisplayError(string error)
    {
        instance.errorText.text = error;
    }

    public static void Clear()
    {
        instance.gameObject.SetActive(false);
    }
}
