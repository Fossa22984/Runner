using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public static NewBehaviourScript Instance;

    [SerializeField] private GameObject _obstacleTopPrefab;
    [SerializeField] private GameObject _obstacleBottomPrefab;
    [SerializeField] private GameObject _obstacleFullPrefab;
   // [SerializeField] private GameObject _rampPrefab;
    [SerializeField] private GameObject _coinPrefab;

    [SerializeField] private List<GameObject> _maps = new List<GameObject>();
    [SerializeField] private List<GameObject> _activeMaps = new List<GameObject>();

    [SerializeField] private int _itemSpace = 15;
    [SerializeField] private int _itemCountInMap = 5;

    enum CoinsStyle { Line, Jump, Ramp };
    enum TrackPos { Left = -1, Center, Right };
    [SerializeField] public float laneOffset = 1f;

    [SerializeField] private int coinsCountInItem = 10;
    [SerializeField] private float coinsHeight = 0.5f;

    struct MapItem
    {
        public GameObject Obstacle;
        public TrackPos TrackPos;
        public CoinsStyle CoinsStyle;

        public void SetValues(GameObject obstacle, TrackPos trackPos, CoinsStyle coinsStyle)
        {
            this.Obstacle = obstacle;
            this.TrackPos = trackPos;
            this.CoinsStyle = coinsStyle;
        }
    }


    private void Awake()
    {
        Instance = this;

        _maps.Add(MakeMap1());
        _maps.Add(MakeMap1());
        foreach (var map in _maps)
        {
            map.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_maps.Add(MakeMap1());
        //_maps.Add(MakeMap1());
        //_maps.Add(MakeMap1());
        //foreach (var map in _maps)
        //{
        //    map.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (ChunkGenerator.Instance._speed == 0) return;

        foreach (var map in _activeMaps)
        {
            map.transform.position -= new Vector3(0, 0, ChunkGenerator.Instance._speed * Time.deltaTime);

        }
        if (_activeMaps[0].transform.position.z < -_itemCountInMap * _itemSpace)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }
    void RemoveFirstActiveMap()
    {
        _activeMaps[0].SetActive(false);
        _maps.Add(_activeMaps[0]);
        _activeMaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while (_activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }
    void AddActiveMap()
    {
        int r = Random.Range(0, _maps.Count);
        GameObject go = _maps[r];
        go.SetActive(true);
        foreach (Transform item in go.transform)
        {
            item.gameObject.SetActive(true);
        }
        go.transform.position = _activeMaps.Count > 0 ?
            _activeMaps[_activeMaps.Count - 1].transform.position + Vector3.forward * _itemCountInMap * _itemSpace : new Vector3(0, 0, 10);

        _maps.RemoveAt(r);
        _activeMaps.Add(go);
    }


    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < _itemCountInMap; i++)
        {
            item.SetValues(null, TrackPos.Center, CoinsStyle.Line);

            //if (i == 2)
            //{
            //    item.SetValues(_rampPrefab, TrackPos.Left, CoinsStyle.Ramp);

            //}
            if (i == 3)
            {
                item.SetValues(_obstacleBottomPrefab, TrackPos.Right, CoinsStyle.Jump);

            }
            else if (i == 4)
            {
                item.SetValues(_obstacleBottomPrefab, TrackPos.Right, CoinsStyle.Jump);

            }

            Vector3 obstaclePos = new Vector3((int)item.TrackPos * laneOffset, 0, i * _itemSpace);
            CreateCoins(item.CoinsStyle, obstaclePos, result);

            if (item.Obstacle != null)
            {
                GameObject go = Instantiate(item.Obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    void CreateCoins(CoinsStyle coinStyle, Vector3 position, GameObject parent)
    {
        Vector3 coinPosition = Vector3.zero;
        if (coinStyle == CoinsStyle.Line)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPosition.y = coinsHeight;
                coinPosition.z = i * ((float)_itemSpace / coinsCountInItem);
                GameObject go = Instantiate(_coinPrefab, coinPosition + position, Quaternion.identity);
                go.transform.SetParent(parent.transform);
            }
        }
        else if (coinStyle == CoinsStyle.Jump)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPosition.y = Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 3, coinsHeight);
                coinPosition.z = i * ((float)_itemSpace / coinsCountInItem);
                GameObject go = Instantiate(_coinPrefab, coinPosition + position, Quaternion.identity);
                go.transform.SetParent(parent.transform);
            }
        }
        //else if (coinStyle == CoinsStyle.Line)
        //{
        //    for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
        //    {
        //        coinPosition.y = coinsHeight;
        //        coinPosition.z = i * ((float)_itemSpace / coinsCountInItem);
        //        GameObject go = Instantiate(_coinPrefab, coinPosition + position, Quaternion.identity);
        //        go.transform.SetParent(parent.transform);
        //    }
        //}
    }
}
