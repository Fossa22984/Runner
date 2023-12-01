using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public const float LengthChunk = 12f;
    public const float ChunkDeleteDistance = -15f;
    public const float StartOfSpawn = 15f;
    public const float LaneOffset = 1f;
    public const float VertexOfParabola = 2.5f;
    public const int Reward = 100;

    public const float StartingSpeed = 8f;
    public const int SpeedChangeStep = 20;

    public static readonly List<char> CharsToReplace = new List<char> { '.', '$', '[', ']', '#', '/' };

    public static bool IsMobilePlatform()
    {
        return Application.isMobilePlatform;
    }

    public static float GetPointParabola(int i)
    {
        return -1 / 2f * Mathf.Pow(i, 2) + VertexOfParabola;
    }

    public static bool CheckInternetConnection()
    {
        NetworkReachability reachability = Application.internetReachability;

        if (reachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection!");
            return false;
        }
        else
        {
            Debug.Log("Internet connection is available.");
            return true;
        }
    }
}