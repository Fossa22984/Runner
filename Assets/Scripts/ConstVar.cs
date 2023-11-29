using UnityEngine;

public static class ConstVar
{
    public const float LengthChunk = 12f;
    public const float ChunkDeleteDistance = -15f;
    public const float StartOfSpawn = 15f;
    public const float LaneOffset = 1f;
    public const float VertexOfParabola = 2.5f;
    public const int Reward = 100;

    public const float StartingSpeed = 10f;
    public const int SpeedChangeStep = 20;

    public static float GetPointParabola(int i)
    {
        return -1 / 2f * Mathf.Pow(i, 2) + VertexOfParabola;
    }
}