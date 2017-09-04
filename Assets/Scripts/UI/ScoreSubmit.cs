using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreSubmit : MonoBehaviour
{
    private string name = null;
    private bool canSubmit = true;
    [SerializeField] private Text text;

    public void Enable()
    {
        text.text = "Submit";
        canSubmit = true;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SubmitScore()
    {
        if (!string.IsNullOrEmpty(name) && canSubmit)
        {
            text.text = "Submitted";
            canSubmit = false;
            StartCoroutine(SendPost());
        }
    }

    private IEnumerator SendPost()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://score.papertopixels.tk/a.php", new Dictionary<string, string>
        {
            { "game", "tilt" },
            { "user", name },
            { "score", GameController.time.ToString() },
            { "mapid", LevelLoader.MapId.ToString() }
        }))
        {
            yield return www.Send();
        }
    }
}
