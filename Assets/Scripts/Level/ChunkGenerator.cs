using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private List<GameObject> _chunks = new List<GameObject>();

    void Start()
    {
        ResetChunks();
    }

    void Update()
    {
        if (_gameManager.IsPause) return;

        foreach (var chunk in _chunks)       
            chunk.transform.position -= new Vector3(0, 0, _gameManager.Speed * Time.deltaTime);
        
        if (_chunks[0].transform.position.z < -15)
        {
            PoolManager.PutObject(_chunks[0]);
            _chunks.RemoveAt(0);

            CreateNextChunk();
        }
    }

    private void CreateNextChunk()
    {
        var chunk = PoolManager.GetObject(_chunkPrefab);

        var position = Vector3.zero;
        if (_chunks.Count > 0)
            position = _chunks[_chunks.Count - 1].transform.position + new Vector3(0, 0, 12);
        
        chunk.transform.position = position;
        chunk.SetActive(true);
        _chunks.Add(chunk);
    }

    public void ResetChunks()
    {
        while (_chunks.Count > 0)
        {
            PoolManager.PutObject(_chunks[0]);
            _chunks.RemoveAt(0);
        }

        for (int i = 0; i < _gameManager.ChunkCount; i++)
            CreateNextChunk();     
    }
}