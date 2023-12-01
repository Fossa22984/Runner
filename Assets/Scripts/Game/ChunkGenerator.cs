using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _chunks = new List<GameObject>();
    private List<GameObject> _activeChunks = new List<GameObject>();

    [SerializeField] private int _chunksCount;

    void Start()
    {
        ResetChunks();
    }

    void Update()
    {
        if (GameManager.IsPause) return;

        foreach (var chunk in _activeChunks)
            chunk.transform.position -= new Vector3(0, 0, GameSpeedController.Speed * Time.deltaTime);

        if (_activeChunks.Count != 0 && _activeChunks[0].transform.position.z < Helper.ChunkDeleteDistance)
        {
            PoolManager.PutObject(_activeChunks[0]);
            _activeChunks.RemoveAt(0);

            CreateNextChunk();
        }
    }

    public void ResetChunks()
    {
        while (_activeChunks.Count > 0)
        {
            PoolManager.PutObject(_activeChunks[0]);
            _activeChunks.RemoveAt(0);
        }

        for (int i = 0; i < _chunksCount; i++)
            CreateNextChunk();
    }

    private void CreateNextChunk()
    {
        var randomChunk = Random.Range(0, _chunks.Count);
        var chunk = PoolManager.GetObject(_chunks[randomChunk]);

        var position = Vector3.zero;
        if (_activeChunks.Count > 0)
            position = _activeChunks[_activeChunks.Count - 1].transform.position + new Vector3(0, 0, Helper.LengthChunk);

        chunk.transform.position = position;
        chunk.SetActive(true);
        _activeChunks.Add(chunk);
    }
}