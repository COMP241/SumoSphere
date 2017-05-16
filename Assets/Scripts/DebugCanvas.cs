using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour
{
    private static DebugCanvas instance;

    [SerializeField] private Text debugText;

    void Start()
    {
        if (instance == null)
            instance = GetComponent<DebugCanvas>();
        else
            Destroy(gameObject);
    }

    public static void SetText(string text)
    {
        instance.debugText.text = text;
    }
}
