using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private string _idleAnimation;
    [SerializeField] private string _runAnimation;
    [SerializeField] private string _jumpAnimation;
    [SerializeField] private string _rollAnimation;
    [SerializeField] private string _stumbleAnimation;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //        SwitchToIdle();
    //    if (Input.GetKeyDown(KeyCode.S))
    //        SwitchToRun();
    //    if (Input.GetKeyDown(KeyCode.D))
    //        SwitchToJump();
    //    if (Input.GetKeyDown(KeyCode.F))
    //        SwitchToRoll();
    //    if (Input.GetKeyDown(KeyCode.G))
    //        SwitchToStumble();
    //}

    public void SwitchToIdle()
    {
        _animator.Play(_idleAnimation);
    }

    public void SwitchToRun()
    {
        _animator.Play(_runAnimation);
    }

    public void SwitchToJump()
    {
        _animator.Play(_jumpAnimation);
    }

    public void SwitchToRoll()
    {
        _animator.Play(_rollAnimation);
    }

    public void SwitchToStumble()
    {
        _animator.Play(_stumbleAnimation);
    }
}
