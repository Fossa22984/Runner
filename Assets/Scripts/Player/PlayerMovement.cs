using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private PlayerState _playerState;

    public void MoveHorizontal(HorizontalMovement direction)
    {
        var vector3 = new Vector3(transform.position.x + (int)direction, transform.position.y, transform.position.z);
        _rigidbody.DOMove(vector3, 0.1f);
    }

    public void Jump()
    {
        _playerState.IsJumping = true;
        _animationController.SwitchToJump();
        transform.DOJump(transform.position, 1f, 1, 0.75f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            if (!PlayerController.IsDead)
                _animationController.SwitchToRun();
            else _animationController.SwitchToIdle();
            _playerState.IsJumping = false;
        });
    }

    public void Roll()
    {
        _playerState.IsRolling = true;
        _collider.height = _collider.height / 2;
        _collider.center = new Vector3(_collider.center.x, _collider.center.y / 2, _collider.center.z);
        _animationController.SwitchToRoll();

        DOVirtual.DelayedCall(0.5f, () =>
        {
            _collider.height = _collider.height * 2;
            _collider.center = new Vector3(_collider.center.x, _collider.center.y * 2, _collider.center.z);

            if (!PlayerController.IsDead)
                _animationController.SwitchToRun();
            else _animationController.SwitchToIdle();
            _playerState.IsRolling = false;
        });
    }
}