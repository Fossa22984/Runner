using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour
{
    const float SwipeThreshold = 50;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 position);
    public ClickDelegate ClickEvent;

    public enum Direction { Left, Right, Up, Down };
    private bool[] _swipes = new bool[4];
    private Vector2 _startTouch, _swipeDelta;
    private bool _touchMoved;

    private Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    private bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    private bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    private bool GetTouch() { return Input.GetMouseButton(0); }

    public static SwipeController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TouchBegan())
        {
            _startTouch = TouchPosition();
            _touchMoved = true;
        }
        else if (TouchEnded() && _touchMoved)
        {
            SendSwipe();
            _touchMoved = false;
        }

        _swipeDelta = Vector2.zero;
        if (_touchMoved && GetTouch())
        {
            _swipeDelta = TouchPosition() - _startTouch;
        }

        if (_swipeDelta.magnitude > SwipeThreshold)
        {
            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
            {
                _swipes[(int)Direction.Left] = _swipeDelta.x < 0;
                _swipes[(int)Direction.Right] = _swipeDelta.x > 0;
            }
            else
            {
                _swipes[(int)Direction.Down] = _swipeDelta.y < 0;
                _swipes[(int)Direction.Up] = _swipeDelta.y > 0;
            }
            SendSwipe();
        }

    }

    private void SendSwipe()
    {
        if (_swipes[0] || _swipes[1] || _swipes[2] || _swipes[3])
        {
            Debug.Log($"{_swipes[0]} | {_swipes[1]} | {_swipes[2]} | {_swipes[3]}");
            MoveEvent?.Invoke(_swipes);
        }
        else
        {
            Debug.Log("Click");
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }

    private void Reset()
    {
        _startTouch = _swipeDelta = Vector2.zero;
        _touchMoved = false;
        for (int i = 0; i < _swipes.Length; i++)
        {
            _swipes[i] = false;
        }
    }
}