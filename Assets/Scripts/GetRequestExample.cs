using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequestExample : MonoBehaviour
{
    private ImageMap map;
    
    IEnumerator Start()
    {
        int id = 0; // Change this for user inputted id
        using (UnityWebRequest www = UnityWebRequest.Get("http://papermap.tk/api/map/" + id))
        {
            yield return www.Send();

            if (!www.isError)
            {
                map = ImageMap.FromJson(www.downloadHandler.text);
                DoStuff();
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    void DoStuff()
    {
        // Now do stuff with map, for example, turn this class into a "LevelGenerator" MonoBehaviour and start creating objects in the level based off the map data
        Debug.Log(map.ToString());
    }
}