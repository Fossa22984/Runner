using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field: SerializeField] public TrackPosition Position { get; set; }
    [field: SerializeField] public bool IsJumping { get; set; }
    [field: SerializeField] public bool IsRolling { get; set; }

    public PlayerState()
    {
        IsJumping = false;
        IsRolling = false;
        Position = TrackPosition.Center;
    }

    public PlayerState(bool isJumping, bool isRolling)
    {
        IsJumping = isJumping;
        IsRolling = isRolling;
        Position = TrackPosition.Center;
    }

    public bool CheckState()
    {
        if (!IsJumping && !IsRolling) return true;
        return false;
    }

    public void SetTrackPosition(HorizontalMovement direction)
    {
        if (direction == HorizontalMovement.Left)
        {
            if (Position == TrackPosition.Center)
                Position = TrackPosition.Left;
            else if (Position == TrackPosition.Right)
                Position = TrackPosition.Center;
        }

        if (direction == HorizontalMovement.Right)
        {
            if (Position == TrackPosition.Center)
                Position = TrackPosition.Right;
            else if (Position == TrackPosition.Left)
                Position = TrackPosition.Center;
        }
    }
}