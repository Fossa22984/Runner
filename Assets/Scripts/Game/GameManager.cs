using Assets.Scripts.DataBase;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsPause { get; private set; } = true;

    public event Action LogOutEvent;

    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameSpeedController _gameSpeedController;
    [SerializeField] private Menu _menu;
    [SerializeField] private PlayerRepository _player;

    private void Awake()
    {
        _playerController.ResetLevelEvent += GameOver;
    }

    private void OnEnable()
    {
        _menu.gameObject.SetActive(true);
        _playerController.gameObject.SetActive(true);
        _menu.SetData(0, _player.Player.Score.BestScore, _player.Player.Score.Balance);
    }

    public void StartGame()
    {
        _playerController.StartGame();

        IsPause = false;
        SwipeController.Instance.enabled = true;
    }

    public async Task GameOver()
    {
        IsPause = true;
        SwipeController.Instance.enabled = false;

        _playerController.ResetPlayerAnimation();
        await _player.ChangeUserData();
        _menu.ShowViewGameOver(await _player.GetLeaders());
    }
    public void ContinueGame()
    {
        _player.SetBalance(Helper.Reward);
        _playerController.ResetPlayer();
        _levelManager.ResetChunks();
        _menu.HideViewGameOver();
    }

    public void ResetGame()
    {
        _menu.HideViewGameOver();
        _menu.SetData(0, _player.Player.Score.BestScore, _player.Player.Score.Balance);

        IsPause = true;
        SwipeController.Instance.enabled = false;

        _playerController.ResetPlayer();
        _levelManager.ResetChunks();
        _gameSpeedController.Reset();
    }

    public async void QuitGame()
    {
        LogOutEvent?.Invoke();
        ResetGame();
        await _player.LogOut();

        _menu.gameObject.SetActive(false);
        _playerController.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    ~GameManager()
    {
        if (LogOutEvent != null)
        {
            foreach (Delegate d in LogOutEvent.GetInvocationList())
                LogOutEvent -= (Action)d;
        }
    }
}