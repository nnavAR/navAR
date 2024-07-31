using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NavigationHandler : MonoBehaviour
{
    private string url = "http://localhost:5000/navigation";

    void Start()
    {
        StartCoroutine(FetchNavigationData());
    }

    IEnumerator FetchNavigationData()
    {
        while (true)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                #if UNITY_2020_2_OR_NEWER
                // Unity 2020.2 and newer
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    ProcessResponse(webRequest.downloadHandler.text);
                }
                #else
                // Unity 2020.1 and older
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    ProcessResponse(webRequest.downloadHandler.text);
                }
                #endif
            }

            yield return new WaitForSeconds(5); // Polling interval, adjust as needed
        }
    }

    void ProcessResponse(string jsonResponse)
    {
        // Process the JSON response
        Debug.Log("Received: " + jsonResponse);
        // You can parse and handle the data here
    }
}
