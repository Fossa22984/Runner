using Assets.Scripts.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static bool IsPause { get; private set; } = true;
    public static float Speed { get; private set; } = 10f;

    [SerializeField] private ChunkGenerator _chunkGenerator;
    [SerializeField] private ObstacleGenerator _obstacleGenerator;
    [SerializeField] private CoinGenerator _coinGenerator;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private List<InitializePoolData> _initializePool = new List<InitializePoolData>();
    [SerializeField] private Transform _parentForPool;
    [SerializeField] private DeathZone _deathZone;

    [SerializeField] private Menu _menu;

    [SerializeField] private PlayerRepository _player;
    private float _elapsedTime = 0;

    void Update()
    {
        if (IsPause) return;

        _elapsedTime += Time.deltaTime;
        var score = (int)(_elapsedTime * Speed);
        _menu.SetData(score, _player.Player.Score.BestScore, _player.Player.Score.Balance);
        _player.SetBestScore(score);
    }

    private void Awake()
    {
        PreparePool();
        // _deathZone.ResetLevelEvent += ResetLevel;
        _deathZone.ResetLevelEvent += GameOver;
        _coinGenerator.SetCoinEvent += SetCoin;
    }

    public void StartLevel()
    {
        _menu.SetData(0, _player.Player.Score.BestScore, _player.Player.Score.Balance);
        //ElapsedTime = 0;
        _playerController.StartLevel();
        // _obstacleGenerator.ResetChunks();
        IsPause = false;
        SwipeController.Instance.enabled = true;
    }

    public async Task GameOver()
    {
        IsPause = true;
        SwipeController.Instance.enabled = false;
        _playerController.ResetPlayer();
        await _player.ChangeUserData();
        _menu.ShowLeaderboard(await _player.GetLeaders());
    }

    public void ResetLevel()
    {
        _menu.HideLeaderboard();


        _menu.SetData(0, _player.Player.Score.BestScore, _player.Player.Score.Balance);
        IsPause = true;
        SwipeController.Instance.enabled = false;
        _elapsedTime = 0;

        //ElapsedTime = 0;
        //_playerController.ResetPlayer();
        _chunkGenerator.ResetChunks();
        _obstacleGenerator.ResetChunks();
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