using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private CoinGenerator _coinGenerator;

    [SerializeField] private List<GameObject> _obstacles = new List<GameObject>();
    private List<GameObject> _activeObstacles = new List<GameObject>();

    [SerializeField] private int _itemSpace = 5;
    [SerializeField] private int _itemCountInMap = 5;

    [SerializeField] private int _obstaclesCount;

    void Start()
    {
        ResetChunks();
    }

    void Update()
    {
        if (GameManager.IsPause) return;

        foreach (var chunk in _activeObstacles)
            chunk.transform.position -= new Vector3(0, 0, GameSpeedController.Speed * Time.deltaTime);

        if (_activeObstacles.Count != 0 && _activeObstacles[0].transform.position.z < Helper.ChunkDeleteDistance)
        {
            PoolManager.PutObject(_activeObstacles[0]);
            _activeObstacles.RemoveAt(0);

            CreateNextChunk();
        }
    }

    public void ResetChunks()
    {
        while (_activeObstacles.Count > 0)
        {
            PoolManager.PutObject(_activeObstacles[0]);
            _activeObstacles.RemoveAt(0);
        }

        _coinGenerator.Reset();

        for (int i = 0; i < _obstaclesCount; i++)
            CreateNextChunk();
    }

    private void CreateNextChunk()
    {
        var randomChunk = Random.Range(0, _obstacles.Count);
        var chunk = PoolManager.GetObject(_obstacles[randomChunk]);

        chunk.transform.position = _activeObstacles.Count > 0 ?
            _activeObstacles[_activeObstacles.Count - 1].transform.position + Vector3.forward * _itemCountInMap * _itemSpace : new Vector3(0, 0, Helper.StartOfSpawn);
        chunk.SetActive(true);
        _activeObstacles.Add(chunk);

        var res = chunk.GetComponent<Obstacle>();
        if (res != null)
        {
            Vector3 obstaclePos = new Vector3((int)res.TrackPosition * Helper.LaneOffset - 0.25f, 0, chunk.transform.position.z);
            _coinGenerator.CreateCoins(res.Style, obstaclePos);
        }
    }
}
