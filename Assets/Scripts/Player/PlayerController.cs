using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public delegate Task ResetLevelDelegate();
    public ResetLevelDelegate ResetLevelEvent;

    [SerializeField] private AnimationController _animationController;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private float _laneChangeSpeed = 15;
    [SerializeField] private float _jumpPower = 15;
    [SerializeField] private float _jumpGravity = -40;
    [SerializeField] private float _laneOffset = 1f;
    [SerializeField] private float _realGravity = -9.8f;
    [SerializeField] private float _pointStart;
    [SerializeField] private float _pointFinish;
    [SerializeField] private float _lastVectorX;
    private Vector3 _startGamePosition;
    private Coroutine _movingCoroutine;
    private bool _isMoving = false;
    private bool _isJumping = false;
    private bool _isRolling = false;
    private bool _isDead = false;


    private void Start()
    {
        _startGamePosition = transform.position;
        SwipeController.Instance.MoveEvent += MovePlayer;
    }

    private void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeController.Direction.Left] && _pointFinish > -_laneOffset)
        {
            MoveHorizontal(-_laneChangeSpeed);
        }
        if (swipes[(int)SwipeController.Direction.Right] && _pointFinish < _laneOffset)
        {
            MoveHorizontal(_laneChangeSpeed);
        }
        if (swipes[(int)SwipeController.Direction.Up] && !_isJumping)
        {
            Jump();
        }
        if (swipes[(int)SwipeController.Direction.Down] && !_isRolling)
        {
            Roll();
        }
    }

    void Jump()
    {
        _isJumping = true;
        _animationController.SwitchToJump();
        _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, _jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    void Roll()
    {
        _collider.height = _collider.height / 2;
        _collider.center = new Vector3(_collider.center.x, _collider.center.y / 2, _collider.center.z);
        _isRolling = true;
        _animationController.SwitchToRoll();
        Invoke(nameof(StopRollCoroutine), 0.5f);
    }

    private void StopRollCoroutine()
    {
        _isRolling = false;
        _collider.height = _collider.height * 2;
        _collider.center = new Vector3(_collider.center.x, _collider.center.y * 2, _collider.center.z);
        _animationController.SwitchToRun();
    }

    private IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);

        } while (_rigidbody.velocity.y != 0);
        _isJumping = false;
        Physics.gravity = new Vector3(0, _realGravity, 0);
        if(!_isDead)
        _animationController.SwitchToRun();
        else _animationController.SwitchToIdle();
    }

    void MoveHorizontal(float speed)
    {
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;
        if (_isMoving)
        {
            StopCoroutine(_movingCoroutine);
            _isMoving = false;
        }
        _movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        _isMoving = true;
        while (Mathf.Abs(_pointStart - transform.position.x) < _laneOffset)
        {
            yield return new WaitForFixedUpdate();
            _rigidbody.velocity = new Vector3(vectorX, _rigidbody.velocity.y, 0);
            _lastVectorX = vectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(_pointStart, _pointFinish), Mathf.Max(_pointStart, _pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        _rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(_pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -10, _rigidbody.velocity.z);
        }
        _isMoving = false;

    }

    public void StartLevel()
    {
        _animationController.SwitchToRun();
    }

    public void ResetPlayerAnimation()
    {
        _animationController.SwitchToIdle();
    }

    public void ResetPlayer()
    {
        _isDead = false;
        _rigidbody.velocity = Vector3.zero;
        _pointStart = 0;
        _pointFinish = 0;

        _animationController.SwitchToIdle();
       // StartCoroutine(LerpMotion());

        _rigidbody.DOMove(_startGamePosition, 0.1f);

        //      transform.position = _startGamePosition;
    }

    private IEnumerator LerpMotion()
    {
        Debug.Log($"[{GetType().Name}][LerpMotion] LerpMotion started");
       var _timeCounter = 0f;
        while (_timeCounter < 1f)
        {
            var normalizedTime = _timeCounter / 1f;
            transform.position = Vector3.Lerp(transform.position, _startGamePosition, normalizedTime);
            _timeCounter += Time.deltaTime;
            yield return null;
        }

        Debug.Log($"[{GetType().Name}][LerpMotion] LerpMotion completed");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _isDead=true;
             ResetLevelEvent?.Invoke();
        }
    }
}