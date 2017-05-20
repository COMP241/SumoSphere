using UnityEngine;

public class GameEndCanvas : MonoBehaviour
{
    private static GameEndCanvas instance;

    // Editor Fields

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<GameEndCanvas>();
        else
            Destroy(gameObject);
        Hide();
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
