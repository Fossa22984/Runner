using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<InitializePoolData> _initializePool = new List<InitializePoolData>();
    [SerializeField] private Transform _parentForPool;

    [SerializeField] private ChunkGenerator _chunkGenerator;
    [SerializeField] private ObstacleGenerator _obstacleGenerator;
    private void Awake()
    {
        PreparePool();
    }

    public void ResetChunks()
    {
        _chunkGenerator.ResetChunks();
        _obstacleGenerator.ResetChunks();
    }

    private void PreparePool()
    {
        PoolManager.SetParentForPoolObjects(_parentForPool);
        foreach (var poolData in _initializePool)
            PoolManager.InitializePool(poolData.Prefab, poolData.Count);
    }
}