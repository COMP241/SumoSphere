using UnityEngine;

public class StartInactive : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
