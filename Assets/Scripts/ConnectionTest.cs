using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ConnectionTest
{
    public static bool successfulOnLastCheck { get; private set; }
    public static float lastTimeChecked;
    public static IEnumerator GetStatus(string url = "http://www.google.com")
    {
        UnityWebRequest request = new UnityWebRequest(url);
        yield return request.SendWebRequest();
        successfulOnLastCheck = request.error == null;
        lastTimeChecked = Time.realtimeSinceStartup;
    }
}
