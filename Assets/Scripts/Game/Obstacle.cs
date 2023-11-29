using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public Style Style { get; private set; }
    [field: SerializeField] public TrackPosition TrackPosition { get; private set; }
}