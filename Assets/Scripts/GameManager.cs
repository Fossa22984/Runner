using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsPause { get; private set; } = true;

    public static float Speed { get; private set; } = 10f;

    [SerializeField] private ChunkGenerator _chunkGenerator;
    [SerializeField] private ObstacleGenerator _obstacleGenerator;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private List<InitializePoolData> _initializePool = new List<InitializePoolData>();
    [SerializeField] private Transform _parentForPool;

    [SerializeField] private TextMeshProUGUI _timerText;

    //public float ElapsedTime { get; private set; }

    //void Update()
    //{
    //    if (IsPause) return;

    //    ElapsedTime += Time.deltaTime;
    //    _timerText.text = ((int)(ElapsedTime*Speed)).ToString();
    //}

    //public string GetTimerText(float elapsedTime)
    //{
    //    int minutes = Mathf.FloorToInt(elapsedTime / 60);
    //    int seconds = Mathf.FloorToInt(elapsedTime % 60);
    //    return string.Format("{0:00}:{1:00}", minutes, seconds);
    //}

    private void Awake()
    {
        PreparePool();
    }

    public void StartLevel()
    {
        //ElapsedTime = 0;
        _playerController.StartLevel();
        // _obstacleGenerator.ResetChunks();
        IsPause = false;
        SwipeController.Instance.enabled = true;
    }

    public void ResetLevel()
    {
        IsPause = true;
        SwipeController.Instance.enabled = false;

        //ElapsedTime = 0;
        _playerController.ResetPlayer();
        _chunkGenerator.ResetChunks();
        _obstacleGenerator.ResetChunks();

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