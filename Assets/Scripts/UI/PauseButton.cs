using UnityEngine;

class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject[] toggleOnPause;

    public void Pause()
    {
        foreach (GameObject o in toggleOnPause)
            o.SetActive(!o.activeSelf);
        Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
    }
}
