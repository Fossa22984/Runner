using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public static ChunkGenerator Instance;
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private List<GameObject> _chunks=new List<GameObject>();
    [SerializeField] private float _maxSpeed=10;
    [SerializeField] public float _speed=0;
    [SerializeField] private int _maxChunkCount=5;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
       // StartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (_speed == 0) return;

        foreach (var chunk in _chunks)
        {
            chunk.transform.position -=new  Vector3(0,0, _speed*Time.deltaTime);
        }
        if (_chunks[0].transform.position.z < -15)
        {
            Destroy(_chunks[0]);
            _chunks.RemoveAt(0);

            CreateNextChunk();
        }
    }

    private void CreateNextChunk()
    {
        Vector3 position =  Vector3.zero;
        if(_chunks.Count > 0)
        {
            position = _chunks[_chunks.Count - 1].transform.position + new Vector3(0, 0, 12);
        }
        var go=Instantiate(_chunkPrefab, position, Quaternion.identity);
        go.transform.SetParent(transform);
        _chunks.Add(go);
    }

    public void StartLevel()
    {
        _speed=_maxSpeed;
        SwipeController.Instance.enabled = true;
    }

    public void ResetLevel()
    {
        _speed = 0;
        while(_chunks.Count > 0)
        {
            Destroy(_chunks[0]);
            _chunks.RemoveAt(0);
        }
        for (int i = 0; i < _maxChunkCount; i++)
        {
            CreateNextChunk();
        }
        SwipeController.Instance.enabled = false;

        NewBehaviourScript.Instance.ResetMaps();
    }

}
