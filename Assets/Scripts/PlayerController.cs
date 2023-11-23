using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimationController animationController;
    [SerializeField] private Rigidbody rb;
    Vector3 startGamePosition;
    Quaternion startGameRotation;
    Coroutine movingCoroutine;
    bool isMoving = false;
    bool isJumping = false;
    public float laneChangeSpeed = 15;
    public float jumpPower = 15;
    public float jumpGravity = -40;
    [SerializeField] private float laneOffset = 1f;
    [SerializeField] private float realGravity = -9.8f;
    float pointStart;
    float pointFinish;
    float lastVectorX;

    private void Start()
    {
        laneOffset = NewBehaviourScript.Instance.laneOffset;
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        SwipeController.Instance.MoveEvent += MovePlayer;
    }

    private void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeController.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeController.Direction.Right] && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
        if (swipes[(int)SwipeController.Direction.Up] && !isJumping)
        {
            Jump();
        }
    }

    void Jump()
    {
        isJumping = true;
        animationController.SwitchToJump();
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    private IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);

        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
        animationController.SwitchToRun();
    }

    void MoveHorizontal(float speed)
    {
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;
        if (isMoving)
        {
            StopCoroutine(movingCoroutine);
            isMoving = false;
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();
            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            lastVectorX = vectorX;
            float x= Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position=new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        isMoving = false;

    }


    public void StartGame() { }// animator.SetTrigger("Run"); }

    public void StartLevel()
    {

        ChunkGenerator.Instance.StartLevel();
      animationController.SwitchToRun();
    }

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        //animator.applyRootMotion = true;
        animationController.SwitchToIdle();
        transform.position = startGamePosition;
        transform.rotation = startGameRotation;
        ChunkGenerator.Instance.ResetLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
        }
        if(other.gameObject.tag == "Lose")
        {
            ResetGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if(collision.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-lastVectorX);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane")
        {
            if(rb.velocity.x==0 && !isJumping)
            {
                rb.velocity=new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
    }

    //// [SerializeField] private CharacterController _controller ;
    //[SerializeField] private Vector3 _startGamePosition;
    //[SerializeField] private Vector3 _targetPosition;
    //[SerializeField] private float _moveSpeed = 3;

    //private int _lineToMove = 1;
    //public float lineDistance = 4;

    //// Update is called once per frame
    //void Update()
    //{
    //    //if (SwipeController.swipeRight)
    //    //{
    //    //    if (_lineToMove < 2) _lineToMove++;
    //    //}

    //    //if (SwipeController.swipeLeft)
    //    //{
    //    //    if (_lineToMove > 0) _lineToMove--;
    //    //}

    //    Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
    //    if (_lineToMove == 0)
    //        targetPosition += Vector3.left * lineDistance;
    //    else if (_lineToMove == 2) targetPosition += Vector3.right * lineDistance;

    //    transform.position = targetPosition;

    //    //transform.Translate(_direction * Time.deltaTime * _moveSpeed, Space.World);
    //}
}
