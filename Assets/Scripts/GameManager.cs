using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ChunkGenerator _chunkGenerator;
    [SerializeField] private ObstacleGenerator _obstacleGenerator;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private List<InitializePoolData> _initializePool = new List<InitializePoolData>();
    [SerializeField] private Transform _parentForPool;

    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int ChunkCount { get; private set; }
    [field: SerializeField] public bool IsPause { get; private set; }


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


    public void StartLevel()
    {
        //ElapsedTime = 0;
        _playerController.StartLevel();
        _obstacleGenerator.ResetMaps();
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
        _obstacleGenerator.ResetMaps();
 
    }

    void Start()
    {
        PreparePool();
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