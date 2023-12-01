using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool IsDead { get; private set; } = false;

    public delegate Task ResetLevelDelegate();
    public ResetLevelDelegate ResetLevelEvent;

    [SerializeField] private AnimationController _animationController;
    [SerializeField] private PlayerMovement _playerController;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PlayerState _playerState;

    private Vector3 _startGamePosition;

    private void Start()
    {
        _startGamePosition = transform.position;
        SwipeController.Instance.MoveEvent += MovePlayer;
    }

    private void Update()
    {
        if (GameManager.IsPause) return;

        if (Input.GetKeyDown(KeyCode.A) && _playerState.Position != TrackPosition.Left && _playerState.CheckState())
        {
            _playerController.MoveHorizontal(HorizontalMovement.Left);
            _playerState.SetTrackPosition(HorizontalMovement.Left);
        }

        if (Input.GetKeyDown(KeyCode.D) && _playerState.Position != TrackPosition.Right && _playerState.CheckState())
        {
            _playerController.MoveHorizontal(HorizontalMovement.Right);
            _playerState.SetTrackPosition(HorizontalMovement.Right);
        }

        if (Input.GetKeyDown(KeyCode.W) && _playerState.CheckState())
            _playerController.Jump();

        if (Input.GetKeyDown(KeyCode.S) && _playerState.CheckState())
            _playerController.Roll();
    }

    private void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeController.Direction.Left] && _playerState.Position != TrackPosition.Left && _playerState.CheckState())
        {
            _playerController.MoveHorizontal(HorizontalMovement.Left);
            _playerState.SetTrackPosition(HorizontalMovement.Left);
        }

        if (swipes[(int)SwipeController.Direction.Right] && _playerState.Position != TrackPosition.Right && _playerState.CheckState())
        {
            _playerController.MoveHorizontal(HorizontalMovement.Right);
            _playerState.SetTrackPosition(HorizontalMovement.Right);
        }

        if (swipes[(int)SwipeController.Direction.Up] && _playerState.CheckState())
            _playerController.Jump();

        if (swipes[(int)SwipeController.Direction.Down] && _playerState.CheckState())
            _playerController.Roll();
    }

    public void StartGame() => _animationController.SwitchToRun();
    public void ResetPlayerAnimation() => _animationController.SwitchToIdle();

    public void ResetPlayer()
    {
        IsDead = false;
        _rigidbody.velocity = Vector3.zero;
        _playerState.Position = TrackPosition.Center;

        _animationController.SwitchToIdle();
        _rigidbody.DOMove(_startGamePosition, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            IsDead = true;
            ResetLevelEvent?.Invoke();
        }
    }

    ~PlayerController()
    {
        if (ResetLevelEvent != null)
        {
            foreach (ResetLevelDelegate d in ResetLevelEvent.GetInvocationList())
                ResetLevelEvent -= d;
        }
    }
}