using Assets.Scripts.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsPause { get; private set; } = true;
    public static float Speed { get; private set; } = ConstVar.StartingSpeed;

    public delegate void LogOutDelegate();
    public LogOutDelegate LogOutEvent;

    [SerializeField] private ChunkGenerator _chunkGenerator;
    [SerializeField] private ObstacleGenerator _obstacleGenerator;
    [SerializeField] private CoinGenerator _coinGenerator;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private List<InitializePoolData> _initializePool = new List<InitializePoolData>();
    [SerializeField] private Transform _parentForPool;
    [SerializeField] private DeathZone _deathZone;

    [SerializeField] private Menu _menu;

    [SerializeField] private PlayerRepository _player;

    private float _speedChange = ConstVar.SpeedChangeStep;
    private float _elapsedTime = 0;

    private void Awake()
    {
        PreparePool();
        _deathZone.ResetLevelEvent += GameOver;
        _coinGenerator.SetCoinEvent += SetCoin;
    }

    void Update()
    {
        if (IsPause) return;

        _elapsedTime += Time.deltaTime;
        var score = (int)(_elapsedTime * Speed);
        if (_elapsedTime >= _speedChange)
        {
            Speed++;
            _speedChange += ConstVar.SpeedChangeStep;
            Debug.Log($"Speed: {Speed}");
        }

        _menu.SetData(score, _player.Player.Score.BestScore, _player.Player.Score.Balance);
        _player.SetBestScore(score);
    }

    private void OnEnable()
    {
        _menu.gameObject.SetActive(true);
        _playerController.gameObject.SetActive(true);
        _menu.SetData(0, _player.Player.Score.BestScore, _player.Player.Score.Balance);
    }

    public void StartLevel()
    {
        _playerController.StartLevel();
        IsPause = false;
        SwipeController.Instance.enabled = true;
    }

    public async Task GameOver()
    {
        IsPause = true;
        SwipeController.Instance.enabled = false;
        _playerController.ResetPlayer();
        _playerController.ResetPlayerAnimation();
        await _player.ChangeUserData();
        _menu.ShowViewGameOver(await _player.GetLeaders());
    }
    public void ContinueGame()
    {
        _player.SetBalance(ConstVar.Reward);

        _playerController.ResetPlayer();
        _chunkGenerator.ResetChunks();
        _obstacleGenerator.ResetChunks();

        _menu.HideViewGameOver();
    }

    public void ResetLevel()
    {
        _menu.HideViewGameOver();

        _menu.SetData(0, _player.Player.Score.BestScore, _player.Player.Score.Balance);
        IsPause = true;
        SwipeController.Instance.enabled = false;
        _elapsedTime = 0;

        _playerController.ResetPlayer();
        _chunkGenerator.ResetChunks();
        _obstacleGenerator.ResetChunks();

        Speed = ConstVar.StartingSpeed;
        _speedChange = ConstVar.SpeedChangeStep;
    }

    public async void QuitGame()
    {
        await _player.LogOut();
        LogOutEvent?.Invoke();
        _menu.gameObject.SetActive(false);
        _playerController.gameObject.SetActive(false);
        gameObject.SetActive(false);

        ResetLevel();
    }

    private void SetCoin()
    {
        _player.SetBalance();
        _menu.SetData((int)(_elapsedTime * Speed), _player.Player.Score.BestScore, _player.Player.Score.Balance);
    }

    private void PreparePool()
    {
        PoolManager.SetParentForPoolObjects(_parentForPool);
        foreach (var poolData in _initializePool)
        {
            PoolManager.InitializePool(poolData.Prefab, poolData.Count);
        }
    }
}